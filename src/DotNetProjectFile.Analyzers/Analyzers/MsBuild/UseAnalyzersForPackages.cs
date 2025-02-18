namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseAnalyzersForPackages() : MsBuildProjectFileAnalyzer(Rule.UseAnalyzersForPackages)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var packageReferences = context.File.Walk().OfType<PackageReferenceBase>().Where(p => p is not PackageVersion);

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
                context.ReportDiagnostic(Descriptor, context.File, analyzer.Name, reference.Name);
            }
        }
    }

    private static readonly NuGet.Analyzer[] Analyzers = NuGet.Packages.All
        .OfType<NuGet.Analyzer>()
        .Where(a => a.Match != "*")
        .ToArray();
}
