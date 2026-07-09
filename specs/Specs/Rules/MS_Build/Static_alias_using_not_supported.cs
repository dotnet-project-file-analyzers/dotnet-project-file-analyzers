namespace Rules.MS_Build.Static_alias_using_not_supported;

public class Reports
{
    [Test]
    public void on_static_alias_using()
       => new StaticAliasUsingNotSupported()
       .ForProject("IncorrectlyOrderedUsingDirectives.cs")
       .HasIssues(
           Issue.ERR("CS8085", "A 'using static' directive cannot be used to declare an alias" /*................*/).WithSpan(05, 20, 05, 23),
           Issue.ERR("Proj0017", "Using directive for 'NamespaceB.Placeholder' can not be both an alias and static").WithSpan(11, 04, 11, 72));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new StaticAliasUsingNotSupported()
        .ForProject(project)
        .HasNoIssues();
}
