using System.Collections.Immutable;

namespace DotNetProjectFile.Analyzers;

public abstract class MsBuildProjectFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics) : DiagnosticAnalyzer
{
    public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [primaryDiagnostic, .. supportedDiagnostics];

    protected virtual IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.All;

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
            if (ApplicableTo.Contains(c.Project.FileType))
            {
                Register(c);
            }
        });

    protected abstract void Register(ProjectFileAnalysisContext context);
}
