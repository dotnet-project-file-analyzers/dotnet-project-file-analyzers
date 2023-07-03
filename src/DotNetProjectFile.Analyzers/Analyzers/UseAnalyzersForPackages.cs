using Microsoft.CodeAnalysis.Diagnostics;

namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages : ProjectFileAnalyzer
{
    public UseAnalyzersForPackages() : base(Rule.UseAnalyzersForPackages) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var projects = context.Project.GetProjects().ToArray();  

        var packageReferences = context.Project.GetProjects()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(group => group.PackageReferences)
            .ToArray();

        foreach (var analyzer in Analyzers)
        {
            if (packageReferences.None(analyzer.IsMatch)
                && context.Compilation.ReferencedAssemblyNames.FirstOrDefault(analyzer.IsMatch) is { } reference)
            {
                context.ReportDiagnostic(Descriptor, analyzer.Package, reference.Name);
            }
        }
    }

    public static readonly PackageAnalyzer[] Analyzers = new PackageAnalyzer[]
    {
        new("FluentAssertions.Analyzers", "FluentAssertions"),
        new("Microsoft.AspNetCore.Components.Analyzers", "Microsoft.AspNetCore"),
        new("Microsoft.Azure.Functions.Analyzers", "Microsoft.Azure.Functions"),
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
