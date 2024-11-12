namespace System.Xml.Linq;

internal static class XContainerExtensions
{
    public static bool ContainsCDATA(this XContainer container)
        => container.Nodes().Any(c => c.NodeType == XmlNodeType.CDATA);
}
