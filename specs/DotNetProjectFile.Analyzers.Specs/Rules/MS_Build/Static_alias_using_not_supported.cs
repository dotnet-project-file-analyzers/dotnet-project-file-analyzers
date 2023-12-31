﻿namespace Rules.MS_Build.Static_alias_using_not_supported;

public class Reports
{
    [Test]
    public void on_static_alias_using()
       => new StaticAliasUsingNotSupported()
       .ForProject("IncorrectlyOrderedUsingDirectives.cs")
       .HasIssues(
           // First set are compiler errors that can be ignored for the sake of testing this analyzer.
           new Issue("CS0400", @"The type or namespace name 'NamespaceA' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(3, 36, 3, 46),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(5, 34, 5, 44),
           new Issue("CS0400", @"The type or namespace name 'NamespaceA' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(1, 21, 1, 31),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(2, 21, 2, 31),
           new Issue("CS0400", @"The type or namespace name 'NamespaceB' could not be found in the global namespace (are you missing an assembly reference?)", DiagnosticSeverity.Error).WithSpan(4, 28, 4, 38),
           new Issue("CS8085", @"A 'using static' directive cannot be used to declare an alias", DiagnosticSeverity.Error).WithSpan(5, 20, 5, 23),

           new Issue("Proj0017", @"Using directive for 'NamespaceB.Placeholder' can not be both an alias and static.").WithSpan(11, 5, 11, 72));
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
