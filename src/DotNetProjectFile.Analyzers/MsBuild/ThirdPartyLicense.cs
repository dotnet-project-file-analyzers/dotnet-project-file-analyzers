using DotNetProjectFile.NuGet;
using DotNetProjectFile.Text;

namespace DotNetProjectFile.MsBuild;

public sealed class ThirdPartyLicense(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Version => Attribute();

    public string? Hash => Attribute();

    public bool IsMatch(PackageVersionInfo info)
        => Glob.TryParse(Include) is { } glob
        && glob.IsMatch(info.Name)
        && VersionMatch(info);

    private bool VersionMatch(PackageVersionInfo info)
        => Version is null
        || info.Version == Version;
}
