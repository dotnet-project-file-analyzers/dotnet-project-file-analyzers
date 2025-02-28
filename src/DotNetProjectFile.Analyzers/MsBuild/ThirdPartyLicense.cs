using DotNetProjectFile.Text;

namespace DotNetProjectFile.MsBuild;

public sealed class ThirdPartyLicense(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Version => Attribute();

    public string? Hash => Attribute();

    public bool IsMatch(PackageReferenceBase refererence)
        => Glob.TryParse(Include) is { } glob
        && glob.IsMatch(refererence.Include!)
        && VersionMatch(refererence);

    private bool VersionMatch(PackageReferenceBase refererence)
        => Version is null
        || (refererence.Version == Version
        || (refererence is PackageReference { VersionOverride.Length: > 0 } package
        && package.VersionOverride == Version));
}
