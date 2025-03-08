namespace Rules.MS_Build.Define_properties_once;

public class Reports
{
    [Test]
    public void Properties_defined_before() => new DefinePropertiesOnce()
        .ForProject("PropertiesTwice.cs")
        .HasIssues(
            Issue.WRN("Proj0011", "Property <ImplicitUsings> has been already defined").WithSpan(/*...*/ 05, 04, 05, 44),
            Issue.WRN("Proj0011", "Property <TargetFrameworks> has been already defined").WithSpan(/*.*/ 12, 04, 12, 54),
            Issue.WRN("Proj0011", "Property <Nullable> has been already defined").WithSpan(/*.........*/ 13, 04, 13, 36),
            Issue.WRN("Proj0011", "Property <ProductName> has been already defined").WithSpan(/*......*/ 22, 04, 22, 66));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_double_properties(string project) => new DefinePropertiesOnce()
        .ForProject(project)
        .HasNoIssues();
}
