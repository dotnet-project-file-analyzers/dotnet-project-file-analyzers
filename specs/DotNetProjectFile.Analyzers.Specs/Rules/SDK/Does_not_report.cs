namespace Rules.SDK.Does_not_report;

public class On
{
    [Test]
    public void Missing_OutputType() => new DefineOutputType()
        .ForSdkProject("DotnetProjectFilesSdk")
        .HasNoIssues();

    [Test]
    public void Project_not_available_as_additional() => new AddAdditionalFile()
        .ForSdkProject("DotnetProjectFilesSdk")
        .HasNoIssues();

    [Test]
    public void Unresolved_Project() => new GuardUnsupported()
        .ForSdkProject("DotnetProjectFilesSdk")
        .HasNoIssues();
}
