namespace DotNetProjectFile.NuGet.Configuration;

public sealed class Clear(XElement element, Node parent, NuGetConfigFile configFile)
    : Node(element, parent, configFile);
