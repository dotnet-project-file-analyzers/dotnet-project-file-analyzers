namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectsRequireSdk() : MsBuildProjectFileAnalyzer(
    Rule.TestProjectsRequireSdk,
    Rule.UsingMicrosoftNetTestSdkImpliesTestProject)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var isTest = context.Project.IsTestProject();
        var hasSdk = context.Project
            .SelfAndImports()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(g => g.PackageReferences)
            .Any(r => r.IncludeOrUpdate == NuGet.Packages.Microsoft_NET_Test_Sdk.Name);

        if (isTest && !hasSdk)
        {
            context.ReportDiagnostic(Rule.TestProjectsRequireSdk, context.Project);
        }
        else if (!isTest && hasSdk)
        {
            context.ReportDiagnostic(Rule.UsingMicrosoftNetTestSdkImpliesTestProject, context.Project);
        }
    }
}
