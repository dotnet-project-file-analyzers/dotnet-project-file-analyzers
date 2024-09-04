namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectsRequireSdk() : MsBuildProjectFileAnalyzer(Rule.TestProjectsRequireSdk)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.IsTestProject() && context.Project
            .SelfAndImports()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(g => g.PackageReferences)
            .None(r => r.IncludeOrUpdate == "Microsoft.NET.Test.Sdk"))
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
