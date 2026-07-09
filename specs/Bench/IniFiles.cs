using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;
using TestData;

namespace Bench;

[MemoryDiagnoser(true)]
public class IniFiles
{
    private static readonly SourceText ini_0027 = Files.Text("ini-0027-lines.ini");
    private static readonly SourceText ini_0036 = Files.Text("ini-0036-lines.ini");
    private static readonly SourceText ini_1220 = Files.Text("ini-1220-lines.ini");

    [Benchmark(Baseline = true)]
    public IniFile _0027() => IniFile.Parse(new(default, ini_0027));

    [Benchmark]
    public IniFile? _0036() => IniFile.Parse(new(default, ini_0036));

    [Benchmark]
    public IniFile _1220() => IniFile.Parse(new(default, ini_1220));
}
