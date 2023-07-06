namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages : ProjectFileAnalyzer
{
    public UseAnalyzersForPackages() : base(Rule.UseAnalyzersForPackages) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.Project.AncestorsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(group => group.PackageReferences)
            .ToArray();

        foreach (var analyzer in Analyzers)
        {
            if (packageReferences.None(analyzer.IsMatch)
                && context.Compilation.ReferencedAssemblyNames.FirstOrDefault(analyzer.IsMatch) is { } reference)
            {
                context.ReportDiagnostic(Descriptor, Location.None, analyzer.Package, reference.Name);
            }
        }
    }

    private static readonly PackageAnalyzer[] Analyzers = new PackageAnalyzer[]
    {
        new("FluentAssertions.Analyzers", "FluentAssertions"),
        new("Microsoft.AspNetCore.Components.Analyzers", "Microsoft.AspNetCore"),
        new("Microsoft.Azure.Functions.Analyzers", "Microsoft.Azure.Functions"),
        new("Microsoft.CodeAnalysis.Analyzers", "Microsoft.CodeAnalysis"),
        new("Microsoft.EntityFrameworkCore.Analyzers", "Microsoft.EntityFrameworkCore"),
        new("MongoDB.Analyzer", "MongoDB"),
        new("NUnit.Analyzers", "NUnit"),
        new("Qowaiv.Analyzers.CSharp", "Qowaiv"),
        new("SerilogAnalyzer", "Serilog"),
        new("xunit.analyzers", "xunit"),
    };

    public sealed record PackageAnalyzer
    {
        public PackageAnalyzer(string package, string match)
        {
            Package = package;
            Match = match;
        }

        public string Package { get; }

        public string Match { get; }

        public bool IsMatch(Xml.PackageReference reference)
            => string.Equals(reference.Include, Package, StringComparison.OrdinalIgnoreCase);

        public bool IsMatch(AssemblyIdentity assembly)
            => assembly.Name.StartsWith(Match, StringComparison.OrdinalIgnoreCase)
            && (assembly.Name.Length == Match.Length
                || assembly.Name[Match.Length] == '.');
    }
}
