namespace DotNetProjectFile.NuGet;

public sealed record Analyzer : Package
{
    public Analyzer(string name, string match = "*", string? language = null) : base(name, language, true)
    {
        Match = match;
    }

    public string Match { get; }

    public bool IsApplicable(string compilationLanguage)
        => Language is null || Language == compilationLanguage;

    public bool IsAnalyzerFor(AssemblyIdentity assembly)
        => IsAnalyzerFor(assembly.Name);

    public bool IsAnalyzerFor(CachedPackage pkg)
        => IsAnalyzerFor(pkg.Name);

    public bool IsAnalyzerFor(PackageReferenceBase reference)
        => IsAnalyzerFor(reference.IncludeOrUpdate);

    public bool IsAnalyzerFor(string name)
        => name.StartsWith(Match, StringComparison.OrdinalIgnoreCase)
        && (name.Length == Match.Length
            || name[Match.Length] == '.');
}
