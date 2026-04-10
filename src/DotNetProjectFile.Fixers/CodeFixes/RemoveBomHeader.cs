using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Threading.Tasks;

namespace DotNetProjectFile.CodeFixes;

public sealed class RemoveBomHeader : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ["Proj3000"];

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
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
    {
        using var stream = new FileInfo(document.FilePath).OpenRead();
        if (stream is { Length: > 3, CanRead: true, CanSeek: true })
        {
            stream.Position = 3;
            return document.WithText(SourceText.From(stream));
        }
        else return document;
    }

    /// <inheritdoc />
    public sealed override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
}
