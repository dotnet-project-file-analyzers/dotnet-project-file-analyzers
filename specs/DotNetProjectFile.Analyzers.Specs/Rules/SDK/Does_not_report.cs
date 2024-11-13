namespace Rules.SDK.Does_not_report;

public class On 
{
    [Test]
    public void Missing_OutputType() => new DefineOutputType()
        .ForSDkProject("DotnetProjectFilesSdk")
        .HasNoIssues();

    [Test]
    public void Project_not_available_as_additional() => new AddAdditionalFile()
        .ForSDkProject("DotnetProjectFilesSdk")
        .HasNoIssues();

    [Test]
    public void Unresolved_Project() => new GuardUnsupported()
        .ForSDkProject("DotnetProjectFilesSdk")
        .HasNoIssues();
}
