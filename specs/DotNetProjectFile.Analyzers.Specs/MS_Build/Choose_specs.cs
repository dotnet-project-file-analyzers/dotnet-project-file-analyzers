using DotNetProjectFile;
using DotNetProjectFile.Diagnostics;

namespace Rules.MS_Build.Choose_specs;

public class Project_contains
{
    [Test]
    public void items_under_Choose()
        => new NodeReporter()
        .ForProject("ChooseWhen.cs")
        .HasIssues(
            new Issue("Proj9999", "Found TargetFrameworks.").WithSpan(03, 5, 03, 54),
            new Issue("Proj9999", "Found Nullable.")/*....*/.WithSpan(04, 5, 04, 31),
            new Issue("Proj9999", "Found NuGetAudit.")/*..*/.WithSpan(10, 9, 10, 37),
            new Issue("Proj9999", "Found NuGetAudit.")/*..*/.WithSpan(18, 9, 18, 38),
            new Issue("Proj9999", "Found Folder.")/*......*/.WithSpan(13, 9, 13, 34),
            new Issue("Proj9999", "Found Folder.")/*......*/.WithSpan(21, 9, 21, 39));
    
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    private sealed class NodeReporter : MsBuildProjectFileAnalyzer
    {
        public NodeReporter() 
            : base(Rule.New(9999, "", "Found {0}.", "", [], Category.Reliability)) { }

        protected override void Register(ProjectFileAnalysisContext context)
        {
            foreach (var prop in context.Project.PropertyGroups.SelectMany(p => p.Children))
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
            foreach (var prop in context.Project.ItemGroups.SelectMany(p => p.Children))
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
        }
    }
}
