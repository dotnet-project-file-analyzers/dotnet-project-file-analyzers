namespace DotNetProjectFile.NuGet.Configuration;

public sealed class PackageSourceMapping(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile);
