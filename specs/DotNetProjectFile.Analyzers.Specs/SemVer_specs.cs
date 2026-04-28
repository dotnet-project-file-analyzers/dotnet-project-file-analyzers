using DotNetProjectFile;

namespace Specs.SemVer_specs;

public class Parses
{
    [TestCase(null)]
    [TestCase("")]
    public void Empty(string? str)
        => SemVer.TryParse(str).Should().BeNull();

    [TestCase("  3.1.41596")]
    [TestCase("3.1.41596")]
    [TestCase("3.1.41596\t")]
    [TestCase("  3.1.41596 \r\n")]
    public void With_trimming_whitespace(string str)
        => SemVer.TryParse(str).Should().Be(new SemVer(3, 1, 41596));

    [TestCase("1.0.0", 1, 0, 0)]
    [TestCase("1.2.0", 1, 2, 0)]
    [TestCase("1.2.9", 1, 2, 9)]
    [TestCase("2.71828.182845904", 2, 71828, 182845904)]
    public void Major_Minor_Patch(string str, int major, int minor,  int patch)
        => SemVer.TryParse(str).Should().Be(new SemVer(major, minor, patch));

    [Test]
    public void PreRelease_suffix()
        => SemVer.TryParse("1.1.0-rc.1").Should().Be(new SemVer(1, 1, 0, preRelease: "rc.1"));

    [Test]
    public void Metadata_suffix()
      => SemVer.TryParse("1.1.0+e471d15").Should().Be(new SemVer(1, 1, 0, buildMetadata: "e471d15"));

    [Test]
    public void PreRelease_and_metadata_suffix()
       => SemVer.TryParse("1.1.0-rc.1+e471d15").Should().Be(new SemVer(1, 1, 0, "rc.1", "e471d15"));

    [TestCase("a1.3.3", "Major contains non-digit")]
    [TestCase("01.3.3", "Leading zero for major")]
    [TestCase("1.03.3", "Leading zero for minor")]
    [TestCase("1.3.03", "Leading zero for path")]
    public void Unsucessfully(string str, string because) 
        => SemVer.TryParse(str).Should().BeNull(because);
}
