namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineSingleTargetFramework() : MsBuildProjectFileAnalyzer(Rule.DefineSingleTargetFramework)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (TargetFrameworksInInmport(context.File)) return;

        foreach (var frameworks in context.File.PropertyGroups
            .SelectMany(p => p.TargetFrameworks)
            .Where(f => f.Value.Count <= 1))
        {
            context.ReportDiagnostic(Descriptor, frameworks);
        }
    }

    /// <remarks>
    /// If any import defines <see cref="TargetFrameworks"/> its values can not
    /// be overridden with <see cref="TargetFramework"/>.
    /// </remarks>>
    private static bool TargetFrameworksInInmport(MsBuildProject project)
        => project.Imports
            .SelectMany(i => i.Project.PropertyGroups)
            .SelectMany(p => p.TargetFrameworks)
            .Any();
}
