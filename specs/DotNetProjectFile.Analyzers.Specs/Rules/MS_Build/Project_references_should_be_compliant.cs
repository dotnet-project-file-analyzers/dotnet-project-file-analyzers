using DotNetProjectFile.MsBuild;

namespace Rules.MS_Build.Project_references_should_be_compliant;

public class Detects_conflicts
{
    public class Depending_on
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
    }

    public class Non_test_projects
    {
        [TestCase(OutputType.Kind.Library)]
        [TestCase(OutputType.Kind.Exe)]
        [TestCase(OutputType.Kind.WinExe)]
        public void benchmark_projects(OutputType.Kind outputKind)
        {
            var proj = new ProjectReferenceInfo
            {
                IsTestProject = false,
                OutputType = outputKind,

            };
            var dep = new ProjectReferenceInfo
            {
                IsBenchmarkProject = true,
            };
            proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.IsBenchmarkProject);
        }
    }

    public class Packables
    {
        [Test]
        public void depend_on_non_packables()
        {
            var proj = new ProjectReferenceInfo { IsPackable = true };
            var dep = new ProjectReferenceInfo { IsPackable = false };
            proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.IsNotPackable);
        }
    }
}

public class Detects_no_conflicts_for
{
    public class Benchmarsk_projects
    {
        [TestCase(OutputType.Kind.Library, true, false)]
        [TestCase(OutputType.Kind.Library, false, true)]
        [TestCase(OutputType.Kind.Library, false, false)]
        [TestCase(OutputType.Kind.Exe, true, false)]
        [TestCase(OutputType.Kind.Exe, false, true)]
        [TestCase(OutputType.Kind.Exe, false, false)]
        [TestCase(OutputType.Kind.WinExe, false, false)]
        public void depending_on(OutputType.Kind outputKind, bool isTestProject, bool isPackable)
        {
            var proj = new ProjectReferenceInfo
            {
                IsBenchmarkProject = true,
            };
            var dep = new ProjectReferenceInfo
            {
                IsTestProject = isTestProject,
                IsPackable = isPackable,
                OutputType = outputKind,
            };
            proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.None);
        }
    }

    public class Non_packables
    {
        [Test]
        public void depending_on_packables()
        {
            var proj = new ProjectReferenceInfo { IsPackable = false };
            var dep = new ProjectReferenceInfo { IsPackable = true };

            proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.None);
        }
    }

    public class Test_projects
    {
        [TestCase(OutputType.Kind.Exe)]
        [TestCase(OutputType.Kind.AppContainerExe)]
        [TestCase(OutputType.Kind.WinExe)]
        public void executables_for_tests(OutputType.Kind type)
        {
            var proj = new ProjectReferenceInfo { IsTestProject = true };
            var dep = new ProjectReferenceInfo { OutputType = type };

            proj.ConflictsWith(dep).Should().Be(ProjectReferenceConflict.None);
        }
    }
}
