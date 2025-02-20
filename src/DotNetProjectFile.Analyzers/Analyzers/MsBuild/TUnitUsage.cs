using DotNetProjectFile.NuGet;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Handles rules about the use of TUnit.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class TUnitUsage() : MsBuildProjectFileAnalyzer(
    Rule.TUnitTestProjectMustBeExe,
    Rule.TUnitTestProjectShouldNotIncludeSdk)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.Walk().OfType<PackageReference>().None(IsTUnit)) { return; }

        if (context.File.Property<OutputType>() is not { Value: OutputType.Kind.Exe })
        {
            context.ReportDiagnostic(Rule.TUnitTestProjectMustBeExe, GetOutputType(context.File));
        }

        if (context.File.Walk().OfType<PackageReference>().LastOrDefault(Packages.Microsoft_NET_Test_Sdk.IsMatch) is { } sdk)
        {
            context.ReportDiagnostic(Rule.TUnitTestProjectShouldNotIncludeSdk, sdk);
        }
    }

    private static bool IsTUnit(PackageReference reference) 
        => Packages.TUnit.IsMatch(reference)
        || Packages.TUnit_Engine.IsMatch(reference);

    private static XmlAnalysisNode GetOutputType(Project project)
        => project.Property<OutputType>() is { } output && output.Project == project
        ? output
        : project;
}
