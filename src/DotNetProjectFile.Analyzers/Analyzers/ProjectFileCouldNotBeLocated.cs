using Microsoft.CodeAnalysis.Diagnostics;

namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectFileCouldNotBeLocated : ProjectFileAnalyzer
{
    public ProjectFileCouldNotBeLocated() : base(Rule.ProjectFileCouldNotBeLocated) { }

    protected override void Register(AnalysisContext context)
        => context.RegisterCompilationAction(Locate);

    private void Locate(CompilationAnalysisContext context)
    {
        if (Xml.Project.Create(context.Compilation) is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Location.None, context.Compilation.SourceModule.ContainingAssembly.Name));
        }
    }

    [ExcludeFromCodeCoverage/* Justification = "Analyzer ensure that this is called for other analyzers." */]
    protected override void Register(ProjectFileAnalysisContext context)
    {
        // Empty.
    }
}
