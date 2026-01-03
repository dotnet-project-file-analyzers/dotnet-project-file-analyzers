using DotNetProjectFile.MsBuild;

namespace Specs.MS_Build.SdkVersion_specs;

public class Parses
{
    [Test]
    public void SDK_versions()
        => SdkVersion.Parse("10.0.101").Should().Be(new SdkVersion(10, 0, 101));

    [Test]
    public void SDK_preview_versions()
        => SdkVersion.Parse("6.0.0-preview.5.21302.13").Should().Be(new SdkVersion(6, 0, 0));
}

public class Compares
{
    [Test]
    public void Less_then() 
        => (SdkVersion.NET8 < SdkVersion.NET10).Should().BeTrue();

    [Test]
    public void More_then()
        => (SdkVersion.NET8 > SdkVersion.NET10).Should().BeFalse();
}
