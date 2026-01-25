namespace DotNetProjectFile.NuGet.Configuration;

public sealed class Add(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile)
{
    public string? Key => Attribute();

    public string? Value => Attribute();
}
