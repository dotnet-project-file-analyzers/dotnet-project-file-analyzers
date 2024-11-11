namespace Rules.Use_VB_specific_only_in_VB_context;

public class Reports
{
    [Test]
    public void CSharp_only_context()
        => new UseInVBContextOnly()
        .ForProject("VBOnlyPropertiesCSharp.cs")
        .HasIssues(
            new Issue("Proj0030", "The property <FrameworkPathOverride> is only applicable when using VB.NET and can therefor be removed." /*.*/).WithSpan(03, 4, 03, 29).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <FrameworkPathOverride> is only applicable when using VB.NET and can therefor be removed." /*.*/).WithSpan(06, 4, 06, 29).WithPath("VBOnlyPropertiesCSharp.csproj"),
            new Issue("Proj0030", "The property <NoVBRuntimeReference> is only applicable when using VB.NET and can therefor be removed." /*..*/).WithSpan(04, 4, 04, 53).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <NoVBRuntimeReference> is only applicable when using VB.NET and can therefor be removed." /*..*/).WithSpan(07, 4, 07, 53).WithPath("VBOnlyPropertiesCSharp.csproj"),
            new Issue("Proj0030", "The property <OptionExplicit> is only applicable when using VB.NET and can therefor be removed." /*........*/).WithSpan(05, 4, 05, 39).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <OptionExplicit> is only applicable when using VB.NET and can therefor be removed." /*........*/).WithSpan(08, 4, 08, 39).WithPath("VBOnlyPropertiesCSharp.csproj"),
            new Issue("Proj0030", "The property <OptionInfer> is only applicable when using VB.NET and can therefor be removed." /*...........*/).WithSpan(06, 4, 06, 33).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <OptionInfer> is only applicable when using VB.NET and can therefor be removed." /*...........*/).WithSpan(09, 4, 09, 33).WithPath("VBOnlyPropertiesCSharp.csproj"),
            new Issue("Proj0030", "The property <OptionStrict> is only applicable when using VB.NET and can therefor be removed." /*..........*/).WithSpan(07, 4, 07, 35).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <OptionStrict> is only applicable when using VB.NET and can therefor be removed." /*..........*/).WithSpan(10, 4, 10, 35).WithPath("VBOnlyPropertiesCSharp.csproj"),
            new Issue("Proj0030", "The property <RemoveIntegerChecks> is only applicable when using VB.NET and can therefor be removed." /*...*/).WithSpan(08, 4, 08, 51).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <RemoveIntegerChecks> is only applicable when using VB.NET and can therefor be removed." /*...*/).WithSpan(11, 4, 11, 51).WithPath("VBOnlyPropertiesCSharp.csproj"),
            new Issue("Proj0030", "The property <VbcVerbosity> is only applicable when using VB.NET and can therefor be removed." /*..........*/).WithSpan(09, 4, 09, 40).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <VbcVerbosity> is only applicable when using VB.NET and can therefor be removed." /*..........*/).WithSpan(12, 4, 12, 40).WithPath("VBOnlyPropertiesCSharp.csproj"),
            new Issue("Proj0030", "The property <VbcToolPath> is only applicable when using VB.NET and can therefor be removed." /*...........*/).WithSpan(10, 4, 10, 19).WithPath("Directory.Build.props"),
            new Issue("Proj0030", "The property <VbcToolPath> is only applicable when using VB.NET and can therefor be removed." /*...........*/).WithSpan(13, 4, 13, 19).WithPath("VBOnlyPropertiesCSharp.csproj"));
}

public class Guards
{
    [TestCase("VBOnlyProperties.vb")]
    [TestCase("CompliantVB.vb")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project)
         => new UseInVBContextOnly()
        .ForProject(project)
        .HasNoIssues();
}
