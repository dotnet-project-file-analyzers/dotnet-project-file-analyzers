namespace Rules.MS_Build.Order_package_versions_in_alphabetical_order;

public class Reports
{
    [Test]
    public void on_not_alphabetical_order()
       => new OrderPackageVersionsAlphabetically()
       .ForProject("NonAlphabeticalPackageVersions.cs")
       .HasIssue(
           Issue.WRN("Proj0024", "Package version for 'Qowaiv.Analyzers.CSharp' is not ordered alphabetically and should appear before 'StyleCop.Analyzers'.").WithSpan(9, 4, 9, 72));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OrderPackageVersionsAlphabetically()
        .ForProject(project)
        .HasNoIssues();
}
