using DotNetProjectFile.IO;
using Grammr;
using Microsoft.CodeAnalysis.Text;
using System.IO;

namespace Xamples;

public static class Files
{
    public static Stream Stream(string file)
    {
        var path = $"TestData.Files.{file}";
        return typeof(Files).Assembly.GetManifestResourceStream(path)
            ?? throw new FileNotFoundException("Could not load embbed stream.", path);
    }

    public static GrammrTree Tree(string file)
        => new(IOFile.Parse(file), SourceText.From(Stream(file)));
}
