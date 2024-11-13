namespace Rules.SDK.Does_not_report;

public class On 
{
    [Test]
    public void Missing_OutputType() => new DefineOutputType()
        .ForSDkProject("DotnetProjectFilesSdk")
        .HasNoIssues();
}
