using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetProjectFile.Dummy;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class FirstAnalyzerCSAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MakeConstCS";
    private const string Title = "Variable can be made constant";
    private const string MessageFormat = "Can be made constant";
    private const string Description = "Make Constant";
    private const string Category = "Usage";

    private static DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

        // Only consider local variable declarations that aren't already const.
        if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
        {
            return;
        }

        // Ensure that all variables in the local declaration have initializers that
        // are assigned with constant values.
        foreach (var variable in localDeclaration.Declaration.Variables)
        {
            var initializer = variable.Initializer;
            if (initializer == null)
            {
                return;
            }

            var constantValue = context.SemanticModel.GetConstantValue(initializer.Value);
            if (!constantValue.HasValue)
            {
                return;
            }
        }

        // Perform data flow analysis on the local declaration.
        var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

        // Retrieve the local symbol for each variable in the local declaration
        // and ensure that it is not written outside of the data flow analysis region.
        foreach (var variable in localDeclaration.Declaration.Variables)
        {
            var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable);
            if (dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                return;
            }
        }

        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
    }
}
