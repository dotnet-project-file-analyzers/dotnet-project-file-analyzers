namespace System.Xml.Linq;

internal static class XContainerExtensions
{
    extension(XContainer container)
    {
        public bool ContainsCDATA
            => container.Nodes().Any(c => c.NodeType == XmlNodeType.CDATA);
    }
}
