namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineOutputType : MsBuildProjectFileAnalyzer
{
    public DefineOutputType() : base(Rule.DefineOutputType) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project.IsProject)
        {
            var outputTypes = context.Project
                .AncestorsAndSelf()
                .SelectMany(p => p.PropertyGroups)
                .SelectMany(g => g.OutputType);

            if (outputTypes.None())
            {
                context.ReportDiagnostic(Descriptor, context.Project);
            }
        }
    }
}
