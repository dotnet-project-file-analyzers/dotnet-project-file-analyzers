namespace DotNetProjectFile.NuGet.Configuration;

[DebuggerDisplay("Key = {Key}, Value = {Value}")]
public sealed class Add(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile)
{
    public string? Key => Attribute("key");

    public string? Value => Attribute("value");
}
