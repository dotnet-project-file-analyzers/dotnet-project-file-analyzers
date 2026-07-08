using DotNetProjectFile.IO;
using Grammr;
using Microsoft.CodeAnalysis.Text;

namespace TestData;

public static class Test
{
    public static GrammrTree Tree(string source, string file = "test.txt")
        => new(IOFile.Parse(file), SourceText.From(source));
}
