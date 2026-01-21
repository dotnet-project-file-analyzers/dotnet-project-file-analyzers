using DotNetProjectFile.MsBuild;

namespace Specs.MS_Build.LanguageVersion_specs;

public class Parses
{
    [TestCase("13", 13, 0)]
    [TestCase("13.0", 13, 0)]
    [TestCase("14.0", 14, 0)]
    [TestCase("14.0.7", 14, 0)]
    public void Explicit_versions(string input, int major, int minor)
    {
        var expected = new LanguageVersion(major, minor);
        LanguageVersion.Parse(input).Should().Be(expected);
        expected.IsExplicit.Should().BeTrue();
    }
}

public class not_explicit
{
    [Test]
    public void None() => LanguageVersion.None.IsExplicit.Should().BeFalse();

    [Test]
    public void Default() => LanguageVersion.Default.IsExplicit.Should().BeFalse();

    [Test]
    public void Latest() => LanguageVersion.Latest.IsExplicit.Should().BeFalse();

    [Test]
    public void Latest_major() => LanguageVersion.LatestMajor.IsExplicit.Should().BeFalse();

    [Test]
    public void Preview() => LanguageVersion.Preview.IsExplicit.Should().BeFalse();
}
