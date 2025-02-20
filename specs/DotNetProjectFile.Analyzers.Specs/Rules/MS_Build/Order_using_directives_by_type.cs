namespace Rules.MS_Build.Order_using_directives_by_type;

public class Reports
{
    [Test]
    public void on_not_ordered_by_type()
       => new OrderUsingDirectivesByType()
       .ForProject("IncorrectlyOrderedUsingDirectives.cs")
       .HasIssues(
           Issue.ERR("CS8085", @"A 'using static' directive cannot be used to declare an alias" /*............................................................*/).WithSpan(05, 20, 05, 23),
           Issue.WRN("Proj0018", @"Using Static directive for 'NamespaceB.Placeholder' should appear before Using Alias directive for 'NamespaceA.Placeholder'.").WithSpan(10, 04, 10, 60));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OrderUsingDirectivesByType()
        .ForProject(project)
        .HasNoIssues();
}
