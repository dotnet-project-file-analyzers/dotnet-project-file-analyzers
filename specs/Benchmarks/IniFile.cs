using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;

namespace Benchmarks;

public class IniFile
{
    private static readonly string root = string.Join("/", Enumerable.Repeat("..", 7)) + "/Files/";

    private readonly List<SourceText> Texts = [];
    
    public IniFile()
    {
        string[] files = [ "ini-0027-lines.ini", "ini-0036-lines.ini", "ini-1220-lines.ini" ];
        foreach(var file in files)
        {
            using var stream = new FileStream(root + file, FileMode.Open, FileAccess.Read);
            Texts.Add(SourceText.From(stream));
        }
    }

    [Params(0, 1, 2)]
    public int Index { get; set; }

    [Benchmark]
    public IniFileSyntax Parse() => IniFileSyntax.Parse(Texts[Index]);
}
