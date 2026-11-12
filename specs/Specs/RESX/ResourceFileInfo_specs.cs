using DotNetProjectFile.IO;
using DotNetProjectFile.Resx;
using System.Globalization;

namespace RESX.ResourceFileInfo_specs;

public class Culture
{
    [Test]
    [TestCase("no-dots.resx", "")]
    [TestCase("dutch.nl-NL.resx", "nl-NL")]
    [TestCase("english.en.resx", "en")]
    [TestCase("this.is.not-a-culture.resx", "")]
    [TestCase("Can/Work/With/paths.es.resx", "es")]
    public void Resolved_from_file(IOFile file, CultureInfo culture)
        => new ResourceFileInfo(file).Culture.Should().Be(culture);
}
