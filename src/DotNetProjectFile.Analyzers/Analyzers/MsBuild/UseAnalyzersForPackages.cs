namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages() : MsBuildProjectFileAnalyzer(Rule.UseAnalyzersForPackages)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(group => group.PackageReferences)
            .ToArray();

        var unusedAnalyzers = Analyzers.Where(analyzer
            => analyzer.IsApplicable(context.Compilation.Options.Language)
            && packageReferences.None(analyzer.IsMatch));

        foreach (var analyzer in unusedAnalyzers)
        {
            if (context.Compilation.ReferencedAssemblyNames
                .Where(analyzer.IsMatch)
                .OrderBy(asm => asm.Name.Length)
                .FirstOrDefault() is { } reference)
            {
                context.ReportDiagnostic(Descriptor, context.Project, analyzer.Name, reference.Name);
            }
        }
    }

    private static readonly NuGet.Analyzer[] Analyzers = NuGet.Packages.All
        .OfType<NuGet.Analyzer>()
        .Where(a => a.Match != "*")
        .ToArray();
}
