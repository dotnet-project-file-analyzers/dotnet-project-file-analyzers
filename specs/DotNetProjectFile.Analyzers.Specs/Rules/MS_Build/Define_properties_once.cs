namespace Rules.MS_Build.Define_properties_once;

public class Reports
{
    [Test]
    public void Properties_defined_before()
        => new DefinePropertiesOnce()
        .ForProject("PropertiesTwice.cs")
        .HasIssues(
            new Issue("Proj0011", "Property <ImplicitUsings> has been already defined.").WithSpan(/*...*/ 05, 05, 05, 44),
            new Issue("Proj0011", "Property <TargetFrameworks> has been already defined.").WithSpan(/*.*/ 12, 05, 12, 54),
            new Issue("Proj0011", "Property <Nullable> has been already defined.").WithSpan(/*.........*/ 13, 05, 13, 36),
            new Issue("Proj0011", "Property <ProductName> has been already defined.").WithSpan(/*......*/ 22, 05, 22, 66));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("CompliantVB.vb")]
    [TestCase("CompliantVBPackage.vb")]
    public void Projects_without_double_properties(string project)
         => new DefinePropertiesOnce()
        .ForProject(project)
        .HasNoIssues();
}
