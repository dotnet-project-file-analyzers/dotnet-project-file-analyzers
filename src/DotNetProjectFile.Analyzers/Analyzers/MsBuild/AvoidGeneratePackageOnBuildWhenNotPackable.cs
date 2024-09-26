namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidGeneratePackageOnBuildWhenNotPackable() : MsBuildProjectFileAnalyzer(Rule.AvoidGeneratePackageOnBuildWhenNotPackable)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (IsPotentiallyPackable(context)) return;

        foreach (var node in context.File.Walk().OfType<GeneratePackageOnBuild>())
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }

    private static bool IsPotentiallyPackable(ProjectFileAnalysisContext context)
    {
        var none = true;
        foreach (var isPackable in context.File.WalkBackward().OfType<IsPackable>())
        {
            if (isPackable.Value is true) return true;
            none = false;
        }
        return none;
    }
}
