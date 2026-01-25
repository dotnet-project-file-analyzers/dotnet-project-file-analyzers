namespace DotNetProjectFile.NuGet.Configuration;

public sealed class Unknown(XElement element, Node parent, NuGetConfigFile configFile)
    : Node<string>(element, parent, configFile)
{
    public override string LocalName => Element.Name.LocalName;
}
