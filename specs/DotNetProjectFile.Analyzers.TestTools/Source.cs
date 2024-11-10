using DotNetProjectFile.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.TestTools;

public static class Source
{
    public static SourceText Text(this string str) => SourceText.From(str);

    public static SourceSpan Span(this string str) => new(SourceText.From(str));
}
