namespace System.Xml.Linq;

internal static class XElementExtensions
{
    extension(XElement element)
    {
        [Pure]
        public int Depth
        {
            get
            {
                var depth = 0;
                var parent = element.Parent;
                while (parent is { })
                {
                    depth++;
                    parent = parent.Parent;
                }
                return depth;
            }
        }

        [Pure]
        public bool PreservesSpace => element
            .AncestorsAndSelf()
            .Select(e => e.Attribute(XmlSpace))
            .OfType<XAttribute>()
            .FirstOrDefault()?.Value == "preserve";
    }

    private static readonly XName XmlSpace = XNamespace.Xml + "space";
}
