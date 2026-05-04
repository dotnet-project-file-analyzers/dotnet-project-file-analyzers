using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.MsBuild;

public sealed class PackageReference(XElement element, Node parent, MsBuildProject project)
    : PackageReferenceBase(element, parent, project)
{
    public string? VersionOverride => Attribute();

    public string VersionOrVersionOverride => Version ?? VersionOverride ?? string.Empty;

    public string? PrivateAssets => Attribute() ?? Child();

    public IEnumerable<string> NoWarn => (Attribute() ?? Child())?
        .Split(SemicolonSeparated, StringSplitOptions.RemoveEmptyEntries).Select(id => id.Trim())
        ?? [];

    public override PackageVersionInfo Info => new(IncludeOrUpdate, VersionOrVersionOverride);
}
