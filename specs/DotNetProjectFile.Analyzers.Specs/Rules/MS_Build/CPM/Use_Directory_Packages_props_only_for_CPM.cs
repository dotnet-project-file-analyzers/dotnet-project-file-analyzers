namespace MS_Build.Use_Directory_Packages_props_only_for_CPM;

public class Reports
{
    [Test]
    public void bloated_Directory_Packages_Props()
        => new UseDirectoryPackagesPropsOnlyForCPM()
        .ForProject("BloatedDirectoryPackagesProps.cs")
        .HasIssues(
            new Issue("Proj0807", "As <TargetFramework> is not about Central Package Management it should not be in Directory.Packages.props." /*..*/).WithSpan(06, 04, 06, 53),
            new Issue("Proj0807", "As <OutputType> is not about Central Package Management it should not be in Directory.Packages.props." /*.......*/).WithSpan(07, 04, 07, 36),
            new Issue("Proj0807", "As <PackageReference> is not about Central Package Management it should not be in Directory.Packages.props." /*.*/).WithSpan(23, 04, 23, 64),
            new Issue("Proj0807", "As <AdditionalFiles> is not about Central Package Management it should not be in Directory.Packages.props." /*..*/).WithSpan(28, 04, 28, 38));
}

public class Guards
{
    [Test]
    public void proper_Directory_Packages_Props()
        => new UseDirectoryPackagesPropsOnlyForCPM()
        .ForProject("UseCPM.cs")
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_CPM(string project)
         => new UseDirectoryPackagesPropsOnlyForCPM()
        .ForProject(project)
        .HasNoIssues();
}
