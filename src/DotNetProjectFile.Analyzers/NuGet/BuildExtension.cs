namespace DotNetProjectFile.NuGet;

public sealed record BuildExtension : Package
{
    public BuildExtension(string name, string? language = null)
        : base(name, language, isPrivateAsset: true) { }
}
