namespace Rules.MS_Build.Adopt_preferred_casing;

public class Reports
{
    [Test]
    public void on_alternative_casing() => new AdoptPreferredCasing()
        .ForProject("CaseInsensitive.cs")
        .HasIssues(
            new Issue("Proj0031", "The node <TARGETFRAMEWORK> has a different casing than the preferred one <TargetFramework>."/*...*/).WithSpan(05, 04, 05, 45),
            new Issue("Proj0031", "The node <ComPile> has a different casing than the preferred one <Compile>."/*...................*/).WithSpan(09, 04, 09, 43),
            new Issue("Proj0031", "The node <packagereference> has a different casing than the preferred one <PackageReference>."/*.*/).WithSpan(13, 04, 13, 57));
}

public class Guards
{
    [TestCase("PackagesWithAnalyzers.cs")]
    [TestCase("PackagesWithoutAnalyzers.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new AdoptPreferredCasing()
        .ForProject(project)
        .HasNoIssues();
}
