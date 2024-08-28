using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Diagnostics;

public interface DiagnosticReporter
{
    void ReportDiagnostic(DiagnosticDescriptor descriptor, LinePositionSpan span, params object?[]? messageArgs);
}
