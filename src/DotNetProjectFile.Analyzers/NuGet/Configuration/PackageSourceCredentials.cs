namespace DotNetProjectFile.NuGet.Configuration;

public sealed class PackageSourceCredentials(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile);
