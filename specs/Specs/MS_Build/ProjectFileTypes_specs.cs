using DotNetProjectFile.CodeAnalysis;
using DotNetProjectFile.MsBuild;

namespace MS_Build_ProjectFileTypes_specs;

public class Defines
{
    [Test]
    public void All() => ProjectFileTypes.All.Should().BeEquivalentTo([
        AnalyzerType.MSBuildProject,
        AnalyzerType.MSBuildProps,
        AnalyzerType.DirectoryBuildProps,
        AnalyzerType.DirectoryBuildTargets,
        AnalyzerType.DirectoryPackagesProps,
        AnalyzerType.SDK]);

    [Test]
    public void AllExceptSDK() => ProjectFileTypes.AllExceptSDK.Should().BeEquivalentTo([
       AnalyzerType.MSBuildProject,
        AnalyzerType.MSBuildProps,
        AnalyzerType.DirectoryBuildProps,
        AnalyzerType.DirectoryBuildTargets,
        AnalyzerType.DirectoryPackagesProps]);

    [Test]
    public void AllExceptDirectoryPackages() => ProjectFileTypes.AllExceptDirectoryPackages.Should().BeEquivalentTo([
        AnalyzerType.MSBuildProject,
        AnalyzerType.MSBuildProps,
        AnalyzerType.DirectoryBuildProps,
        AnalyzerType.DirectoryBuildTargets,
        AnalyzerType.SDK]);
}
