namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectsRequireSdk() : MsBuildProjectFileAnalyzer(
    Rule.TestProjectsRequireSdk,
    Rule.UsingMicrosoftNetTestSdkImpliesTestProject)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var isTest = context.File.IsTestProject();
        var hasSdk = context.File
            .Walk().OfType<PackageReference>()
            .Any(r => r.IncludeOrUpdate == NuGet.Packages.Microsoft_NET_Test_Sdk.Name);

        if (isTest && !hasSdk)
        {
            context.ReportDiagnostic(Rule.TestProjectsRequireSdk, context.File);
        }
        else if (!isTest && hasSdk)
        {
            context.ReportDiagnostic(Rule.UsingMicrosoftNetTestSdkImpliesTestProject, context.File);
        }
    }
}
