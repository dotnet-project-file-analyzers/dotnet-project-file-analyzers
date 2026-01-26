namespace DotNetProjectFile.NuGet.Configuration;

public sealed class PackageCredentials(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile);
