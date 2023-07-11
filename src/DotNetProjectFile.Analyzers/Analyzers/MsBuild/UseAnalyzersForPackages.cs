namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages : MsBuildProjectFileAnalyzer
{
    public UseAnalyzersForPackages() : base(Rule.UseAnalyzersForPackages) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.IsProject)
        {
            var packageReferences = context.Project
                .AncestorsAndSelf()
                .SelectMany(p => p.ItemGroups)
                .SelectMany(group => group.PackageReferences)
                .ToArray();

            var unusedAnalyzers = Analyzers
                .Where(analyzer => analyzer.IsApplicable(context.Compilation.Options.Language))
                .Where(analyzer => packageReferences.None(analyzer.IsMatch));

            foreach (var analyzer in unusedAnalyzers)
            {
                if (context.Compilation.ReferencedAssemblyNames
                    .Where(analyzer.IsMatch)
                    .OrderBy(asm => asm.Name.Length)
                    .FirstOrDefault() is { } reference)
                {
                    context.ReportDiagnostic(Descriptor, context.Project, analyzer.Package, reference.Name);
                }
            }
        }
    }

    private static readonly PackageAnalyzer[] Analyzers = new PackageAnalyzer[]
    {
        new("Ardalis.ApiEndpoints.CodeAnalyzers", "Ardalis.ApiEndpoints"),
        new("FakeItEasy.Analyzer.CSharp", "FakeItEasy", LanguageNames.CSharp),
        new("FakeItEasy.Analyzer.VisualBasic", "FakeItEasy", LanguageNames.VisualBasic),
        new("FluentAssertions.Analyzers", "FluentAssertions"),
        new("Libplanet.Analyzers", "Libplanet"),
        new("Lucene.Net.Analysis.Common", "Lucene.Net"),
        new("MassTransit.Analyzers", "MassTransit"),
        new("MessagePackAnalyzer", "MessagePack"),
        new("MessagePipe.Analyzer", "MessagePipe"),
        new("Microsoft.AspNetCore.Components.Analyzers", "Microsoft.AspNetCore"),
        new("Microsoft.Azure.Functions.Analyzers", "Microsoft.Azure.Functions"),
        new("Microsoft.CodeAnalysis.Analyzers", "Microsoft.CodeAnalysis"),
        new("Microsoft.EntityFrameworkCore.Analyzers", "Microsoft.EntityFrameworkCore"),
        new("Microsoft.ServiceHub.Analyzers", "Microsoft.ServiceHub"),
        new("MongoDB.Analyzer", "MongoDB"),
        new("Moq.Analyzers", "Moq"),
        new("NSubstitute.Analyzers.CSharp", "NSubstitute", LanguageNames.CSharp),
        new("NSubstitute.Analyzers.VisualBasic", "NSubstitute", LanguageNames.VisualBasic),
        new("NUnit.Analyzers", "NUnit"),
        new("RuntimeContracts.Analyzer", "RuntimeContracts"),
        new("SerilogAnalyzer", "Serilog"),
        new("xunit.analyzers", "xunit"),
        new("ZeroFormatter.Analyzer", "ZeroFormatter"),
    };

    private sealed record PackageAnalyzer(string Package, string Match, string? Language = null)
    {
        public bool IsApplicable(string compilationLanguage)
            => Language is null || Language == compilationLanguage;

        public bool IsMatch(PackageReference reference)
            => string.Equals(reference.Include, Package, StringComparison.OrdinalIgnoreCase);

        public bool IsMatch(AssemblyIdentity assembly)
            => assembly.Name.StartsWith(Match, StringComparison.OrdinalIgnoreCase)
            && (assembly.Name.Length == Match.Length
                || assembly.Name[Match.Length] == '.');
    }
}
