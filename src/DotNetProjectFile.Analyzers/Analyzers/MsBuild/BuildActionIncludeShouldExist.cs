using System.IO;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class BuildActionIncludeShouldExist() : MsBuildProjectFileAnalyzer(Rule.BuildActionIncludeShouldExist)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.Project.ItemGroups.SelectMany(group => group.BuildActions))
        {
            foreach (var include in node.Include.Where(i => context.Project.Path.Directory.Files(i)?.Any() == false))
            {
                context.ReportDiagnostic(Descriptor, node, include, node.LocalName, Ending(include));
            }
        }
    }

    private string Ending(string include)
        => include.Contains('?') || include.Contains('*')
            ? "match any files"
            : "exist";
}
