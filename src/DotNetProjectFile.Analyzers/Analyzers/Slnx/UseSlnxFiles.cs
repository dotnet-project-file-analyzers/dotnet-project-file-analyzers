namespace DotNetProjectFile.Analyzers.Slnx;

/// <summary>Implements <see cref="Rule.SLNX.UseSlnxFiles"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseSlnxFiles() : MsBuildProjectFileAnalyzer(Rule.SLNX.UseSlnxFiles)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.SDK;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Options.AdditionalFiles.None(f => IOFile.Parse(f.Path).Extension.IsMatch(".slnx"))
            && !Net90OrUp(context))
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }

    private static bool Net90OrUp(ProjectFileAnalysisContext context)
    {
        foreach (var text in context.Options.AdditionalFiles)
        {
            if (ProjectFiles.Global.MsBuildProject(text) is { } project
                && (project.PropertyGroups.Children<TargetFramework>().Select(tf => tf.Value).Any(Net90OrUp)
                    || project.PropertyGroups.Children<TargetFrameworks>().Select(tf => tf.Value).SelectMany(v => v).Any(Net90OrUp)))
            {
                return true;
            }
        }
        return false;

        static bool Net90OrUp(string? str)
            => str.Contains(TargetFramework.net9_0, StringComparison.OrdinalIgnoreCase)
            || str.Contains(TargetFramework.net10_0, StringComparison.OrdinalIgnoreCase);
    }
}
