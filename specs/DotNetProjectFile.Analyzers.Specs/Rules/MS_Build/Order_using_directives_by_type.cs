namespace Rules.MS_Build.Order_using_directives_by_type;

public class Reports
{
    [Test]
    public void on_not_ordered_by_type()
       => new OrderUsingDirectivesByType()
       .ForProject("IncorrectlyOrderedUsingDirectives.cs")
       .HasIssues(
           // First set are compiler errors that can be ignored for the sake of testing this analyzer.
           new Issue("CS0400", @"The type or namespace name 'NamespaceA' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(3, 36, 3, 46),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(5, 34, 5, 44),
           new Issue("CS0400", @"The type or namespace name 'NamespaceA' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(1, 21, 1, 31),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(2, 21, 2, 31),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(4, 28, 4, 38),
           new Issue("CS8085", @"A 'using static' directive cannot be used to declare an alias", DiagnosticSeverity.Error).WithSpan(5, 20, 5, 23),

           new Issue("Proj0018", @"Using directive for 'NamespaceB' is should appear before Using directive for 'NamespaceB'.").WithSpan(7, 5, 7, 34));
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
