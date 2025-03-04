using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages() : MsBuildProjectFileAnalyzer(Rule.UseAnalyzersForPackages)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.File
            .Walk()
            .OfType<PackageReferenceBase>()
            .Where(p => p is not PackageVersion && !string.IsNullOrWhiteSpace(p.IncludeOrUpdate));

        var directlyReferenced = packageReferences.Select(r => r.IncludeOrUpdate).ToImmutableHashSet();

        var analyzers = GetAnalyzers(context.Compilation.Language);

        foreach (var reference in packageReferences)
        {
            var tree = reference.ResolveCachedPackageDependencyTree();

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

    private static NuGet.Analyzer[] GetAnalyzers(string language)
        => Analyzers
        .Where(analyzer => analyzer.IsApplicable(language))
        .ToArray();

    private static readonly NuGet.Analyzer[] Analyzers = NuGet.Packages.All
        .OfType<NuGet.Analyzer>()
        .Where(a => a.Match != "*")
        .ToArray();
}
