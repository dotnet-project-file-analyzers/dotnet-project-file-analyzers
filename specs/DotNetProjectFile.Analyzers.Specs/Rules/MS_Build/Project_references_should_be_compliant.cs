using DotNetProjectFile.MsBuild;

namespace Rules.MS_Build.Project_references_should_be_compliant;

public class Detects_conflicts_for
{
    [Test]
    public void test_projects()
    {
        var proj = new ProjectReferenceInfo();

        var dep = new ProjectReferenceInfo
        {
            IsTestProject = true,
        };

        proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.IsTestProject);
    }

    [TestCase(OutputType.Kind.Exe)]
    [TestCase(OutputType.Kind.AppContainerExe)]
    [TestCase(OutputType.Kind.WinExe)]
    public void executables(OutputType.Kind type)
    {
        var proj = new ProjectReferenceInfo();

        var dep = new ProjectReferenceInfo
        {
            OutputType = type,
        };

        proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.IsExe);
    }

    [Test]
    public void non_packables_dependencies_of_packables()
    {
        var proj = new ProjectReferenceInfo
        {
            IsPackable = true,
        };

        var dep = new ProjectReferenceInfo
        {
            IsPackable = false,
        };

        proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.IsNotPackable);
    }
}

public class Detects_no_conflicts_for
{
    [Test]
    public void packables_dependencies_for_non_packables()
    {
        var proj = new ProjectReferenceInfo
        {
            IsPackable = false,
        };

        var dep = new ProjectReferenceInfo
        {
            IsPackable = true,
        };

        proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.None);
    }

    [TestCase(OutputType.Kind.Exe)]
    [TestCase(OutputType.Kind.AppContainerExe)]
    [TestCase(OutputType.Kind.WinExe)]
    public void executables_for_tests(OutputType.Kind type)
    {
        var proj = new ProjectReferenceInfo
        {
            IsTestProject = true,
        };

        var dep = new ProjectReferenceInfo
        {
            OutputType = type,
        };

        proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.None);
    }
}
