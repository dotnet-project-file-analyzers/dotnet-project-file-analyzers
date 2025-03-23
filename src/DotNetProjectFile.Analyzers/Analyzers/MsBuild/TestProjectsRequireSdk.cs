using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TestProjectsRequireSdk() : MsBuildProjectFileAnalyzer(
    Rule.TestProjectsRequireSdk,
    Rule.UsingMicrosoftNetTestSdkImpliesTestProject)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var isTest = context.File.IsTestProject();
        var hasSdk = context.File
            .Walk().OfType<PackageReference>()
            .Any(NuGet.Packages.Microsoft_NET_Test_Sdk.IsMatch);
        var hasTUnit = context.File
            .Walk().OfType<PackageReference>()
            .Any(r => Packages.TUnit.IsMatch(r) || Packages.TUnit_Engine.IsMatch(r));

        var hasSdkOrTUnit = hasSdk || hasTUnit;

        if (isTest && !hasSdkOrTUnit)
        {
            context.ReportDiagnostic(Rule.TestProjectsRequireSdk, context.File);
        }
        else if (!isTest && hasSdkOrTUnit)
        {
            context.ReportDiagnostic(Rule.UsingMicrosoftNetTestSdkImpliesTestProject, context.File);
        }
    }
}
