using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Threading.Tasks;

namespace DotNetProjectFile.CodeFixes;

public sealed class RemoveBomHeader : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [Rule.OnlyUseUTF8WithoutBom.Id];

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        if (context.Diagnostics.FirstOrDefault() is { } diagnostic)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    "Remove UTF-8 BOM header",
                    t => UpdateDocument(context.Document, t),
                    "Proj3000.Fix"),
                diagnostic);
        }

        return Task.CompletedTask;
    }

    private static async Task<Document> UpdateDocument(Document document, CancellationToken token)
    {
        using var file = IOFile.Parse(document.FilePath).OpenRead();
        var stream = new MemoryStream();
        _ = await file.ReadAsync(new byte[3], 0, 3, token);
        await file.CopyToAsync(stream);
        stream.Position = 0;
        return document.WithText(SourceText.From(stream));
    }

    /// <inheritdoc />
    public sealed override FixAllProvider? GetFixAllProvider() => null;
}
