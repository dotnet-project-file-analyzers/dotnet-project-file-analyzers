using DotNetProjectFile.Analyzers;

namespace DotNetProjectFile.MsBuild;

/// <summary>Implements <see cref="Rule.UnresolvableImport"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UnresolvableImport() : MsBuildProjectFileAnalyzer(Rule.UnresolvableImport)
{
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var import in context.File.Imports.Where(i => i.Value is null))
        {
            context.ReportDiagnostic(Descriptor, import, import.Element.Attribute("Project").Value);
        }
    }
}
