namespace DotNetProjectFile.NuGet.Configuration;

public sealed class PackageSource(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile)
{
    public string? Key => Attribute("key");

    public Nodes<Package> Packages => new(Children);
}
