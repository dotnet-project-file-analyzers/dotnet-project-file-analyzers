namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.DefineSingleTargetFramework"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineSingleTargetFramework() : MsBuildProjectFileAnalyzer(Rule.DefineSingleTargetFramework)
{
    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (TargetFrameworksInInmport(context.File)) return;

        foreach (var frameworks in context.File.PropertyGroups
            .Children<TargetFrameworks>(NotMultiple))
        {
            context.ReportDiagnostic(Descriptor, frameworks);
        }
    }

    private static bool NotMultiple(TargetFrameworks tfm)
        => tfm.Value.Count <= 1;

    /// <remarks>
    /// If any import defines <see cref="TargetFrameworks"/> its values can not
    /// be overridden with <see cref="TargetFramework"/>.
    /// </remarks>>
    private static bool TargetFrameworksInInmport(MsBuildProject project)
        => project.Imports
            .Select(p => p.Project)
            .SelectMany(i => i.PropertyGroups.Children<TargetFrameworks>())
            .Any();
}
