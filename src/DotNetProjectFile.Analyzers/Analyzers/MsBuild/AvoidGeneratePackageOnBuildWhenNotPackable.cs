namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidGeneratePackageOnBuildWhenNotPackable() : MsBuildProjectFileAnalyzer(Rule.AvoidGeneratePackageOnBuildWhenNotPackable)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (IsPotentiallyPackable(context))
        {
            return;
        }

        var nodes = context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.GeneratePackageOnBuild)
            .ToArray();

        foreach (var node in nodes)
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }

    private static bool IsPotentiallyPackable(ProjectFileAnalysisContext context)
    {
        var nodes = context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.IsPackable)
            .ToArray();

        return nodes.Length == 0
            || Array.Exists(nodes, n => n.Value == true);
    }
}
