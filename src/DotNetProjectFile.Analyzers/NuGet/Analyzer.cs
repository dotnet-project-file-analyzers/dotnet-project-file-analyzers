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

    public bool IsMatch(PackageReference reference) => reference.Include.IsMatch(Name);

    public bool IsMatch(GlobalPackageReference reference) => reference.Include.IsMatch(Name);

    public bool IsMatch(AssemblyIdentity assembly)
        => assembly.Name.StartsWith(Match, StringComparison.OrdinalIgnoreCase)
        && (assembly.Name.Length == Match.Length
            || assembly.Name[Match.Length] == '.');
}
