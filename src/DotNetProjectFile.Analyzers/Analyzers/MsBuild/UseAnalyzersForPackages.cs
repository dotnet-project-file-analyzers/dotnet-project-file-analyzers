using DotNetProjectFile.NuGet;
using System.Collections.Frozen;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.UseAnalyzersForPackages"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages() : MsBuildProjectFileAnalyzer(Rule.UseAnalyzersForPackages)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.File
            .Walk()
            .OfType<PackageReferenceBase>()
            .Where(p => p is not PackageVersion && p.IncludeOrUpdate is { Length: > 0 });

        var analyzers = GetAnalyzers(context.File.Language);

        foreach (var reference in packageReferences.Where(r => !Excluded.Contains(r.IncludeOrUpdate)))
        {
            var tree = GetAllReferencedPackages(reference, context.ManagePackageVersionsCentrally);

            foreach (var pkg in tree)
            {
                var requiredAnalyzers = analyzers.Where(analyzer => analyzer.IsAnalyzerFor(pkg));
                var missingAnalyzers = requiredAnalyzers.Where(analyzer => packageReferences.None(analyzer.IsMatch));

                // Exclude packages that ship the analyzer themselves
                foreach (var analyzer in missingAnalyzers.Where(a => tree.None(p => a.IsMatch(p))))
                {
                    context.ReportDiagnostic(Descriptor, reference, analyzer.Name, reference.IncludeOrUpdate);
                }
            }
        }
    }

    private static IEnumerable<Package> GetAllReferencedPackages(PackageReferenceBase reference, bool cpmEnabled)
        => reference.ResolvePackage(cpmEnabled) is { IsAnalyzerOnly: true } rootPackage
        ? [rootPackage]
        : (IEnumerable<Package>)reference.ResolveCachedPackageDependencyTree(cpmEnabled);

    private static ImmutableArray<Analyzer> GetAnalyzers(Language language)
        => [.. Analyzers.Where(analyzer => analyzer.IsApplicable(language))];

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

    private static readonly FrozenSet<string> Excluded = new HashSet<string>
    {
        "xunit.runner.visualstudio",
    }
    .ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    private sealed record Analyzer(string Name, string Match, Language? Language = null)
    {
        public bool IsApplicable(Language compilationLanguage)
            => Language is null || Language.Value == compilationLanguage;

        public bool IsAnalyzerFor(Package pkg)
            => !pkg.IsAnalyzerOnly
            && IsAnalyzerFor(pkg.Name);

        public bool IsAnalyzerFor(string name)
            => name.StartsWith(Match, StringComparison.OrdinalIgnoreCase)
            && (name.Length == Match.Length
                || name[Match.Length] == '.');

        public bool IsMatch(PackageReferenceBase reference) => reference.Include.IsMatch(Name);

        public bool IsMatch(Package reference) => reference.Name.IsMatch(Name);
    }
}
