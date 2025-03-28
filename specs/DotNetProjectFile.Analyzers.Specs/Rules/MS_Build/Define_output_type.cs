namespace Rules.MS_Build.Define_output_type;

public class Reports
{
    [Test]
    public void on_no_output_type()
       => new DefineOutputType()
       .ForProject("NoOutputType.cs")
       .HasIssue(
           Issue.WRN("Proj0010", "Define the <OutputType> node explicitly"));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefineOutputType()
        .ForProject(project)
        .HasNoIssues();
}
