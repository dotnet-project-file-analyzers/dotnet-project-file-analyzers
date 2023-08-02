namespace Rules.MS_Build.Order_using_directives_in_alphabetical_order;

public class Reports
{
    [Test]
    public void on_not_alphabetical_order()
       => new OrderUsingDirectivesAlphabetically()
       .ForProject("IncorrectlyOrderedUsingDirectives.cs")
       .HasIssues(
           // First set are compiler errors that can be ignored for the sake of testing this analyzer.
           new Issue("CS0400", @"The type or namespace name 'NamespaceA' could not be found in the global namespace (are you missing an assembly reference?)").WithSpan(3, 36, 3, 46).WithSeverity(DiagnosticSeverity.Error),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)").WithSpan(5, 34, 5, 44).WithSeverity(DiagnosticSeverity.Error),
           new Issue("CS0400", @"The type or namespace name 'NamespaceA' could not be found in the global namespace (are you missing an assembly reference?)").WithSpan(1, 21, 1, 31).WithSeverity(DiagnosticSeverity.Error),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)").WithSpan(2, 21, 2, 31).WithSeverity(DiagnosticSeverity.Error),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)").WithSpan(4, 28, 4, 38).WithSeverity(DiagnosticSeverity.Error),
           new Issue("CS8085", @"A 'using static' directive cannot be used to declare an alias").WithSpan(5, 20, 5, 23).WithSeverity(DiagnosticSeverity.Error),

           new Issue("Proj0019", @"Using directive 'NamespaceA' is not ordered alphabetically and should appear before 'NamespaceB'.").WithSpan(9, 5, 9, 34));
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
