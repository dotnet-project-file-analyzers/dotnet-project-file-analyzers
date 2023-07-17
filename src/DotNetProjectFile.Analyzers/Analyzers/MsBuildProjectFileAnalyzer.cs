using System.Collections.Immutable;

namespace DotNetProjectFile.Analyzers;

public abstract class MsBuildProjectFileAnalyzer : DiagnosticAnalyzer
{
    protected MsBuildProjectFileAnalyzer(DiagnosticDescriptor primaryDiagnostic, params DiagnosticDescriptor[] supportedDiagnostics)
        => SupportedDiagnostics = new[] { primaryDiagnostic }.Concat(supportedDiagnostics).ToImmutableArray();

    public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

    protected virtual bool ApplyToProps => true;

    protected DiagnosticDescriptor Descriptor => SupportedDiagnostics[0];

    public sealed override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        Register(context);
    }

    protected virtual void Register(AnalysisContext context)
        => context.RegisterProjectFileAction(c =>
        {
            if (c.Project.IsProject || ApplyToProps)
            {
                Register(c);
            }
        });

    protected abstract void Register(ProjectFileAnalysisContext context);
}
