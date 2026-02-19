namespace DotNetProjectFile.NuGet.Configuration;

[DebuggerDisplay("Pattern = {Pattern}")]
public sealed class Package(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile)
{
    public string? Pattern => Attribute("pattern");
}
