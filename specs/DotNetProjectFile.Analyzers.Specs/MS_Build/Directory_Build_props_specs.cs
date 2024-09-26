using DotNetProjectFile;
using DotNetProjectFile.Diagnostics;

namespace Rules.MS_Build.Directory_Build_props_specs;

public class Project_relies_on
{
    [Test]
    public void Directory_Build_props()
        => new NodeReporter()
        .ForProject("WithDirectoryBuildProps.cs")
        .HasIssue(
            new Issue("Proj9999", "Found Directory.Build.props."));
    
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    private sealed class NodeReporter : MsBuildProjectFileAnalyzer
    {
        public NodeReporter() 
            : base(Rule.New(9999, "", "Found {0}.", "", [], Category.Reliability)) { }

        protected override void Register(ProjectFileAnalysisContext context)
        {
            if (context.File.DirectoryBuildProps is { } prop)
            {
                context.ReportDiagnostic(Descriptor, prop, prop.Path.Name);
            }
        }
    }
}
