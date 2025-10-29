using DotNetProjectFile.BuildAgents;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableContinuousIntegrationBuild() : MsBuildProjectFileAnalyzer(Rule.EnableContinuousIntegrationBuild)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File
            .Walk()
            .OfType<PackageReferenceBase>()
            .Any(p => p is not PackageVersion && p.Include.IsMatch("DotNet.ReproducibleBuilds")))
        {
            return;
        }

        var nodes = context.File.Properties<ContinuousIntegrationBuild>().ToImmutableArray();

        if (nodes.Length <= 0)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (nodes.None(node => node.HasAnyCondition(BuildAgentExtensions.GetActiveAllowedConditions()) && node.Value == true))
        {
            context.ReportDiagnostic(Descriptor, nodes[0]);
        }
    }
}
