namespace Rules.MS_Build.Order_using_directives_in_alphabetical_order;

public class Reports
{
    [Test]
    public void on_not_alphabetical_order()
       => new OrderUsingDirectivesAlphabetically()
       .ForProject("IncorrectlyOrderedUsingDirectives.cs")
       .HasIssues(
           Issue.WRN("CS8085", @"A 'using static' directive cannot be used to declare an alias", DiagnosticSeverity.Error /*.......*/).WithSpan(5, 20, 5, 23),
           Issue.WRN("Proj0019", @"Using directive 'NamespaceA' is not ordered alphabetically and should appear before 'NamespaceB'.").WithSpan(9, 04, 9, 34));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OrderUsingDirectivesAlphabetically()
        .ForProject(project)
        .HasNoIssues();
}
