namespace Rules.MS_Build.Order_using_directives_by_type;

public class Reports
{
    [Test]
    public void on_not_ordered_by_type()
       => new OrderUsingDirectivesByType()
       .ForProject("IncorrectlyOrderedUsingDirectives.cs")
       .HasIssues(
           new Issue("CS8085", @"A 'using static' directive cannot be used to declare an alias", DiagnosticSeverity.Error).WithSpan(5, 20, 5, 23),
           new Issue("Proj0018", @"Using Static directive for 'NamespaceB.Placeholder' should appear before Using Alias directive for 'NamespaceA.Placeholder'.").WithSpan(10, 5, 10, 60));
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
