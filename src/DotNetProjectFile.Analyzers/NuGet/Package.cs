namespace DotNetProjectFile.NuGet;

[Inheritable]
public record Package
{
    public Package(string name, string? language = null, bool isPrivateAsset = false)
    {
        Name = name;
        IsPrivateAsset = isPrivateAsset;
        Language = language;
    }

    public string Name { get; }

    public bool IsPrivateAsset { get; }

    public string? Language { get; }

    public bool IsMatch(PackageReferenceBase reference) => reference.Include.IsMatch(Name);
}
