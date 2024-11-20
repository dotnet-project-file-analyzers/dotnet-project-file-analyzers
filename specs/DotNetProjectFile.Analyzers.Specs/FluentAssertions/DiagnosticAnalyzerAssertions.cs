using FluentAssertions.Primitives;

namespace FluentAssertions;

internal sealed class DiagnosticAnalyzerAssertions(DiagnosticAnalyzer value)
    : ObjectAssertions<DiagnosticAnalyzer, DiagnosticAnalyzerAssertions>(value)
{
    public AndConstraint<DiagnosticAnalyzerAssertions> HaveId(string diagnosticId)
    {
        Subject.SupportedDiagnostics[0].Id.Should().Be(diagnosticId);
        return new(this);
    }
}