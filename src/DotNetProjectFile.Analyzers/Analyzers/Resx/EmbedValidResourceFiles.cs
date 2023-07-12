using System;

namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EmbedValidResourceFiles : ResourceFileAnalyzer
{
    public EmbedValidResourceFiles() : base(Rule.EmbedValidResourceFiles) { }

    protected override void Register(AnalysisContext context)
    => context.RegisterCompilationAction(c =>
    {
        foreach (var resource in DotNetProjectFile.Resx.Resources
            .Resolve(c.Options.AdditionalFiles)
            .Where(r => r.Exception is { }))
        {
            Register(new ResourceFileAnalysisContext(resource, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
        }
    });

    protected override void Register(ResourceFileAnalysisContext context)
    {
        context.ReportDiagnostic(
            Descriptor,
            context.Resource,
            context.Resource.Exception!.Message);
    }
}
