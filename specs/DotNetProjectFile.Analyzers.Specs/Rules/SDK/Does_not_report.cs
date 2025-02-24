namespace Rules.SDK.Does_not_report;

public class On 
{
    [Test]
    public void Missing_OutputType() => new DefineOutputType()
        .ForSDkProject("DotnetProjectFilesSdk")
        .HasNoIssues();

    /// <remarks>Imported files can be ignored.</remarks>>
    [Test]
    public void Project_not_available_as_additional() => new AddAdditionalFile()
        .ForSDkProject("DotnetProjectFilesSdk")
        .HasIssues(
            Issue.WRN("Proj0006", "Add 'DotNetProjectFile.Analyzers.Sdk.props' to the additional files"),
            Issue.WRN("Proj0006", "Add 'DotNetProjectFile.Analyzers.Sdk.targets' to the additional files"));

    [Test]
    public void Unresolved_Project() => new GuardUnsupported()
        .ForSDkProject("DotnetProjectFilesSdk")
        .HasNoIssues();
}
