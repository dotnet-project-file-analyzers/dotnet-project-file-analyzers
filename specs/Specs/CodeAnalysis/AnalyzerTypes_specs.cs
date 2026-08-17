using DotNetProjectFile.CodeAnalysis;
using DotNetProjectFile.IO;

namespace CodeAnalysis.AnalyzerTypes_specs;

public class MsBuild_matches
{
    [TestCase(".net.csproj")]
    [TestCase(@"C:\code\repo\.net.csproj")]
    public void SDK(IOFile file) => AnalyzerTypes.MsBuild(file).Should().Be(AnalyzerType.SDK);

    [TestCase("project.csproj")]
    [TestCase(@"C:\code\repo\project.csproj")]
    public void MSBuildProject(IOFile file) => AnalyzerTypes.MsBuild(file).Should().Be(AnalyzerType.MSBuildProject);

    [TestCase("Directory.Build.props")]
    [TestCase(@"C:\code\repo\Directory.Build.props")]
    [TestCase(@"C:\code\repo\directory.build.props")]
    public void DirectoryBuildProps(IOFile file) => AnalyzerTypes.MsBuild(file).Should().Be(AnalyzerType.DirectoryBuildProps);

    [TestCase("Directory.Build.targets")]
    [TestCase(@"C:\code\repo\Directory.Build.targets")]
    [TestCase(@"C:\code\repo\directory.build.targets")]
    public void DirectoryBuildTargets(IOFile file) => AnalyzerTypes.MsBuild(file).Should().Be(AnalyzerType.DirectoryBuildTargets);

    [TestCase("Directory.Packages.props")]
    [TestCase(@"C:\code\repo\Directory.Packages.props")]
    [TestCase(@"C:\code\repo\directory.packages.props")]
    public void DirectoryPackagesProps(IOFile file) => AnalyzerTypes.MsBuild(file).Should().Be(AnalyzerType.DirectoryPackagesProps);

    [TestCase("shared.props")]
    [TestCase("shared.targets")]
    [TestCase(@"C:\code\repo\shared.props")]
    [TestCase(@"C:\code\repo\shared.targets")]
    public void MSBuildProps(IOFile file) => AnalyzerTypes.MsBuild(file).Should().Be(AnalyzerType.MSBuildProps);

    [TestCase("file.exe")]
    [TestCase("file.dll")]
    [TestCase(@"C:\code\repo\file.cs")]
    [TestCase(@"C:\code\repo\file.vb")]
    public void known_files_only(IOFile file) => AnalyzerTypes.MsBuild(file).Should().BeNull();
}
