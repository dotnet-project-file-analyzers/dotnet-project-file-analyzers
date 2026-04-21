using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DotNetProjectFile.CodeFixes;

public sealed class RemoveBomHeader : CodeFixProvider
{
    private static readonly UTF8Encoding UTF8_no_BOM = new(encoderShouldEmitUTF8Identifier: false);

    /// <inheritdoc />
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ["Proj3000"];

    /// <inheritdoc />
    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        Debugger.Launch();
        if (context.Diagnostics.FirstOrDefault() is { } diagnostic)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    "Remove UTF-8 BOM header",
                    _ => UpdateDocument(context.Document),
                    "Proj3000.Fix"),
                diagnostic);
        }

        return Task.CompletedTask;
    }

    private static async Task<Document> UpdateDocument(Document document)
        => document.WithText(SourceText.From(document.ToString(), UTF8_no_BOM));

    /// <inheritdoc />
    public sealed override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
}
