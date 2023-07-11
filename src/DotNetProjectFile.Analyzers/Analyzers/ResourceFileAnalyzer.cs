using System.Collections.Immutable;

namespace DotNetProjectFile.Analyzers;

public abstract class ResourceFileAnalyzer : DiagnosticAnalyzer
{
    protected ResourceFileAnalyzer(DiagnosticDescriptor primaryDiagnostic, params DiagnosticDescriptor[] supportedDiagnostics)
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
        => context.RegisterResourceFileAction(Register);

    protected abstract void Register(ResourceFileAnalysisContext context);
}
