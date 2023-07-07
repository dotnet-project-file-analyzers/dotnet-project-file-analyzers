using System.Collections.Immutable;

namespace DotNetProjectFile;

public abstract class ProjectFileAnalyzer : DiagnosticAnalyzer
{
    protected ProjectFileAnalyzer(DiagnosticDescriptor primaryDiagnostic, params DiagnosticDescriptor[] supportedDiagnostics)
        => SupportedDiagnostics = new[] { primaryDiagnostic }.Concat(supportedDiagnostics).ToImmutableArray();

    public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

    public DiagnosticDescriptor Descriptor => SupportedDiagnostics[0];

    public sealed override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        Register(context);
    }

    protected virtual void Register(AnalysisContext context)
        => context.RegisterProjectFileAction(Register);

    protected abstract void Register(ProjectFileAnalysisContext context);
}
