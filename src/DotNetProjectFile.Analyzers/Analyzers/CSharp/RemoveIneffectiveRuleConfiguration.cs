using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetProjectFile.Analyzers.CSharp;

/// <summary>
/// Implements
/// <see cref="Rule.RemoveConfigurationNotConfigurableRule"/>
/// <see cref="Rule.RemoveDroppedRuleConfiguration"/>
/// .</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RemoveIneffectiveRuleConfiguration() : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
    [
        Rule.RemoveConfigurationNotConfigurableRule,
        Rule.RemoveDroppedRuleConfiguration,
    ];

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzePragmas, SyntaxKind.CompilationUnit);
        context.RegisterSyntaxNodeAction(AnalyzeAttribute, SyntaxKind.Attribute);
    }

    private static void AnalyzePragmas(SyntaxNodeAnalysisContext context)
    {
        foreach (var code in context.Node.DescendantTrivia()
            .Where(t => t.IsKind(SyntaxKind.PragmaWarningDirectiveTrivia))
            .Select(t => t.GetStructure())
            .OfType<PragmaWarningDirectiveTriviaSyntax>()
            .SelectMany(p => p.ErrorCodes)
            .OfType<IdentifierNameSyntax>())
        {
            Report(context, code, code.Identifier.Text);
        }
    }

    private static void AnalyzeAttribute(SyntaxNodeAnalysisContext context)
    {
        var attribute = (AttributeSyntax)context.Node;

        if (IsSuppressMessage(attribute)
            && CheckId(attribute) is { } checkId
            && context.SemanticModel.GetConstantValue(checkId, context.CancellationToken) is { HasValue: true, Value: string id })
        {
            Report(context, checkId, Trim(id));
        }
    }

    private static void Report(SyntaxNodeAnalysisContext context, SyntaxNode node, string id)
    {
        if (RoslynRules.NotConfigurables.Contains(id))
        {
            context.ReportDiagnostic(Diagnostic.Create(
               Rule.RemoveConfigurationNotConfigurableRule,
               node.GetLocation(),
               id));
        }
        else if (RoslynRules.Dropped.Contains(id))
        {
            context.ReportDiagnostic(Diagnostic.Create(
               Rule.RemoveDroppedRuleConfiguration,
               node.GetLocation(),
               id));
        }
    }

    private static ExpressionSyntax? CheckId(AttributeSyntax attribute) => attribute switch
    {
        _ when attribute.ArgumentList?.Arguments is not { Count: >= 2 }
            => null,

        _ when attribute.ArgumentList.Arguments.FirstOrDefault(a => a.NameColon?.Name.Identifier.Text.IsMatch(nameof(CheckId)) is true) is { } colon
            => colon.Expression,

        _ when attribute.ArgumentList.Arguments[1].NameEquals is null
            => attribute.ArgumentList.Arguments[1].Expression,

        _ => null,
    };

    private static bool IsSuppressMessage(AttributeSyntax attribute)
        => attribute.Name.ToString() is { Length: > 0 } name
        && (name is "SuppressMessage" or "SuppressMessageAttribute"
            || name.EndsWith(".SuppressMessage")
            || name.EndsWith(".SuppressMessageAttribute"));

    private static string Trim(string id)
    {
        var index = id.IndexOf(':');
        return index == -1
            ? id
            : id[..index];
    }
}
