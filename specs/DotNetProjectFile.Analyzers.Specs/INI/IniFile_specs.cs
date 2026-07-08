using DotNetProjectFile.Ini;

namespace Specs.INI.IniFile_specs;

public class Parses
{
    [TestCase("ini-0001-lines.ini", 1)]
    [TestCase("ini-0008-lines.ini", 5)]
    [TestCase("ini-0027-lines.ini", 14)]
    [TestCase("ini-0036-lines.ini", 11)]
    [TestCase("ini-1220-lines.ini", 1177)]
    public void Embedded(string file, int count)
    {
        var tree = Files.Tree(file);
        var ini = IniFile.Parse(tree);
        ini.Sections.SelectMany(s => s.Entries).Should().HaveCount(count);
    }
}
