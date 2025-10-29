using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages() : MsBuildProjectFileAnalyzer(Rule.UseAnalyzersForPackages)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.File
            .Walk()
            .OfType<PackageReferenceBase>()
            .Where(p => p is not PackageVersion && !string.IsNullOrWhiteSpace(p.IncludeOrUpdate));

        var analyzers = GetAnalyzers(context.File.Language);

        foreach (var reference in packageReferences)
        {
            var tree = GetAllReferencedPackages(reference);

            foreach (var pkg in tree)
            {
                var requiredAnalyzers = analyzers.Where(analyzer => analyzer.IsAnalyzerFor(pkg));
                var missingAnalyzers = requiredAnalyzers.Where(analyzer => packageReferences.None(analyzer.IsMatch));

                foreach (var analyzer in missingAnalyzers)
                {
                    context.ReportDiagnostic(Descriptor, reference, analyzer.Name, reference.IncludeOrUpdate);
                }
            }
        }
    }

    private static IEnumerable<Package> GetAllReferencedPackages(PackageReferenceBase reference)
    {
        var rootPackage = reference.ResolvePackage();

        if (rootPackage?.IsAnalyzerOnly == true)
        {
            return [rootPackage];
        }
        else
        {
            return reference.ResolveCachedPackageDependencyTree();
        }
    }

    private static Analyzer[] GetAnalyzers(Language language)
        => Analyzers
        .Where(analyzer => analyzer.IsApplicable(language))
        .ToArray();

    private static readonly Analyzer[] Analyzers =
    [
        new Analyzer("AwesomeAssertions.Analyzers", "AwesomeAssertions"),
        new Analyzer("Ardalis.ApiEndpoints.CodeAnalyzers", "Ardalis.ApiEndpoints"),
        new Analyzer("FakeItEasy.Analyzer.CSharp", "FakeItEasy", Language.CSharp),
        new Analyzer("FakeItEasy.Analyzer.VisualBasic", "FakeItEasy", Language.VisualBasic),
        new Analyzer("AwesomeAssertions.Analyzers", "AwesomeAssertions"),
        new Analyzer("Libplanet.Analyzers", "Libplanet"),
        new Analyzer("Lucene.Net.Analysis.Common", "Lucene.Net"),
        new Analyzer("MassTransit.Analyzers", "MassTransit"),
        new Analyzer("MessagePackAnalyzer", "MessagePack"),
        new Analyzer("MessagePipe.Analyzer", "MessagePipe"),
        new Analyzer("Microsoft.AspNetCore.Components.Analyzers", "Microsoft.AspNetCore.Components"),
        new Analyzer("Microsoft.Azure.Functions.Analyzers", "Microsoft.Azure.Functions"),
        new Analyzer("Microsoft.EntityFrameworkCore.Analyzers", "Microsoft.EntityFrameworkCore"),
        new Analyzer("Microsoft.ServiceHub.Analyzers", "Microsoft.ServiceHub"),
        new Analyzer("MongoDB.Analyzer", "MongoDB"),
        new Analyzer("Moq.Analyzers", "Moq"),
        new Analyzer("NSubstitute.Analyzers.CSharp", "NSubstitute", Language.CSharp),
        new Analyzer("NSubstitute.Analyzers.VisualBasic", "NSubstitute", Language.VisualBasic),
        new Analyzer("NUnit.Analyzers", "NUnit"),
        new Analyzer("RuntimeContracts.Analyzer", "RuntimeContracts"),
        new Analyzer("SerilogAnalyzer", "Serilog"),
        new Analyzer("xunit.analyzers", "xunit"),
        new Analyzer("ZeroFormatter.Analyzer", "ZeroFormatter"),
    ];

    private sealed record Analyzer(string Name, string Match, Language? Language = null)
    {
        public bool IsApplicable(Language compilationLanguage)
            => Language is null || Language.Value == compilationLanguage;

        public bool IsAnalyzerFor(Package pkg)
        {
            if (pkg.IsAnalyzerOnly)
            {
                return false;
            }

            return IsAnalyzerFor(pkg.Name);
        }

        public bool IsAnalyzerFor(string name)
            => name.StartsWith(Match, StringComparison.OrdinalIgnoreCase)
            && (name.Length == Match.Length
                || name[Match.Length] == '.');

        public bool IsMatch(PackageReferenceBase reference) => reference.Include.IsMatch(Name);
    }
}
