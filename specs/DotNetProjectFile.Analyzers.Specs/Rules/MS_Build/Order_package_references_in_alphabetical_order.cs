namespace Rules.MS_Build.Order_package_references_in_alphabetical_order;

public class Reports
{
    [Test]
    public void on_no_is_packable()
       => new OrderPackageReferencesAlphabetically()
       .ForProject("NonAlphabeticalPackageReferences.cs")
       .HasIssue(
           new Issue("Proj0014", "Package 'Qowaiv.Analyzers.CSharp' is not ordered alphabetically and should appear before 'StyleCop.Analyzers'.").WithSpan(9, 5, 9, 70));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OrderPackageReferencesAlphabetically()
        .ForProject(project)
        .HasNoIssues();
}
