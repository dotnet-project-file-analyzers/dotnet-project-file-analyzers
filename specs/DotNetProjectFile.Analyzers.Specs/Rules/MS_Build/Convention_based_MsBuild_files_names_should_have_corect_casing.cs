namespace Rules.MS_Build.Convention_based_MsBuild_files_names_should_have_correct_casing;

public class Reports
{
    [Test]
    public void Faulty_casing() => new ConventionBasedMsBuildFilesNamesShouldHaveCorrectCasing()
        .ForSDkProject("MSBuildFileNameConvention")
        .HasIssues(
            Issue.WRN("Proj0045", "The file .editorconFIG should be named .editorconfig"/*............................*/).WithPath(".editorconFIG"),
            Issue.WRN("Proj0045", "The file .Globalconfig should be named .globalconfig"/*............................*/).WithPath(".Globalconfig"),
            Issue.WRN("Proj0045", "The file .NET.csproj should be named .net.csproj"/*................................*/).WithPath(".NET.csproj"),
            Issue.WRN("Proj0045", "The file directory.build.props should be named Directory.Build.props"/*............*/).WithPath("directory.build.props"),
            Issue.WRN("Proj0045", "The file directory.build.targets should be named Directory.Build.targets"/*........*/).WithPath("directory.build.targets"),
            Issue.WRN("Proj0045", "The file directory.packages.props should be named Directory.Packages.props"/*......*/).WithPath("directory.packages.props"),
            Issue.WRN("Proj0045", "The file nuget.config should be named NuGet.config"/*..............................*/).WithPath("nuget.config"),
            Issue.WRN("Proj0045", "The file packages.lock.JSON should be named packages.lock.json"/*..................*/).WithPath("packages.lock.JSON"),
            Issue.WRN("Proj0045", "The file compatibilitySuppressions.xml should be named CompatibilitySuppressions.xml").WithPath("compatibilitySuppressions.xml")
        );
}
