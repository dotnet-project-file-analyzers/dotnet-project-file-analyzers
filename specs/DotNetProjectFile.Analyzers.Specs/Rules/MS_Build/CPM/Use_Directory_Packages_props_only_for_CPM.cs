namespace MS_Build.Use_Directory_Packages_props_only_for_CPM;

public class Reports
{
    [Test]
    public void bloated_Directory_Packages_Props()
        => new UseDirectoryPackagesPropsOnlyForCPM()
        .ForProject("BloatedDirectoryPackagesProps.cs")
        .HasIssues(
            new Issue("Proj0807", "As <TargetFramework> is not about Central Package Management it should not be in Directory.Packages.props." /*..*/).WithSpan(05, 04, 05, 54),
            new Issue("Proj0807", "As <OutputType> is not about Central Package Management it should not be in Directory.Packages.props." /*.......*/).WithSpan(06, 04, 06, 37),
            new Issue("Proj0807", "As <PackageReference> is not about Central Package Management it should not be in Directory.Packages.props." /*.*/).WithSpan(22, 04, 22, 65),
            new Issue("Proj0807", "As <AdditionalFiles> is not about Central Package Management it should not be in Directory.Packages.props." /*..*/).WithSpan(27, 04, 27, 39));
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
