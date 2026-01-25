namespace DotNetProjectFile.NuGet.Configuration;

public sealed class PackageSources(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile)
{
    public Nodes<Add> Adds => new(Children);
}
