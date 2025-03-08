using DotNetProjectFile;
using DotNetProjectFile.Diagnostics;
using DotNetProjectFile.MsBuild;

namespace Rules.MS_Build.Choose_specs;

public class Project_contains
{
    [Test]
    public void items_under_Choose()
        => new NodeReporter()
        .ForProject("ChooseWhen.cs")
        .HasIssues(
            Issue.WRN("Proj9999", "Found TargetFrameworks").WithSpan(03, 4, 03, 54),
            Issue.WRN("Proj9999", "Found Nullable")/*....*/.WithSpan(04, 4, 04, 31),
            Issue.WRN("Proj9999", "Found NuGetAudit")/*..*/.WithSpan(10, 8, 10, 37),
            Issue.WRN("Proj9999", "Found NuGetAudit")/*..*/.WithSpan(18, 8, 18, 38),
            Issue.WRN("Proj9999", "Found Folder")/*......*/.WithSpan(13, 8, 13, 34),
            Issue.WRN("Proj9999", "Found Folder")/*......*/.WithSpan(21, 8, 21, 39));
    
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    private sealed class NodeReporter : MsBuildProjectFileAnalyzer
    {
        public NodeReporter() 
            : base(Rule.New(9999, "", "Found {0}", "", [], Category.Reliability)) { }

        protected override void Register(ProjectFileAnalysisContext context)
        {
            foreach (var prop in context.File.PropertyGroups.Children<Node>())
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
            foreach (var prop in context.File.ItemGroups.Children<Node>())  
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
        }
    }
}
