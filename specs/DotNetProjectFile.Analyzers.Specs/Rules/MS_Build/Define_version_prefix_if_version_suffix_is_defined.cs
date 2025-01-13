namespace Rules.MS_Build.Define_version_prefix_if_version_suffix_is_defined;

public class Reports
{
    [Test]
    public void on_version_and_prefix()
       => new DefineVersionPrefixIfVersionSuffixIsDefined()
        .ForProject("VersionSuffix.cs")
        .HasIssue(
           Issue.WRN("Proj0246", "Define the <VersionPrefix> node or remove the <VersionSuffix> node.")
           .WithSpan(09, 04, 09, 45));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("VersionPrefix.cs")]
    [TestCase("VersionPrefixAndSuffix.cs")]
    public void Projects_without_issues(string project)
         => new DefineVersionPrefixIfVersionSuffixIsDefined()
        .ForProject(project)
        .HasNoIssues();
}
