namespace Rules.Use_CSharp_specific_only_in_CSharp_context;

public class Reports
{
    [Test]
    public void VB_NET_only_context()
        => new UseInCSharpContextOnly()
        .ForProject("CSharpOnlyPropertiesVB.vb")
        .HasIssues(
            new Issue("Proj0029", "The property <AllowUnsafeBlocks> is only applicable when using C# and can therefor be removed." /*...*/).WithSpan(03, 4, 03, 48).WithPath("Directory.Build.props"),
            new Issue("Proj0029", "The property <AllowUnsafeBlocks> is only applicable when using C# and can therefor be removed." /*...*/).WithSpan(05, 4, 05, 48).WithPath("CSharpOnlyPropertiesVB.vbproj"),
            new Issue("Proj0029", "The property <CheckForOverflowUnderflow> is only applicable when using C# and can therefor be removed.").WithSpan(04, 4, 04, 63).WithPath("Directory.Build.props"),
            new Issue("Proj0029", "The property <CheckForOverflowUnderflow> is only applicable when using C# and can therefor be removed.").WithSpan(06, 4, 06, 63).WithPath("CSharpOnlyPropertiesVB.vbproj"),
            new Issue("Proj0029", "The property <Nullable> is only applicable when using C# and can therefor be removed." /*............*/).WithSpan(05, 4, 05, 31).WithPath("Directory.Build.props"),
            new Issue("Proj0029", "The property <Nullable> is only applicable when using C# and can therefor be removed." /*............*/).WithSpan(07, 4, 07, 31).WithPath("CSharpOnlyPropertiesVB.vbproj"));
}

public class Guards
{
    [TestCase("CSharpOnlyProperties.cs")]
    [TestCase("CompliantVB.vb")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new UseInCSharpContextOnly()
        .ForProject(project)
        .HasNoIssues();
}
