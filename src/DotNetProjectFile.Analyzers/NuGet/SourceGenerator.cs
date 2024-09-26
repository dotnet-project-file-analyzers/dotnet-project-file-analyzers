namespace DotNetProjectFile.NuGet;

public sealed record SourceGenerator : Package
{
    public SourceGenerator(string name, string? language = null)
        : base(name: name, language: language, isPrivateAsset: true)
    {
    }
}
