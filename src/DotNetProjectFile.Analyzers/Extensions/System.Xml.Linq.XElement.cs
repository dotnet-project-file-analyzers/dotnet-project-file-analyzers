namespace System.Xml.Linq;

internal static class XElementExtensions
{
    [Pure]
    public static int Depth(this XElement element)
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

    [Pure]
    public static bool PreservesSpace(this XElement element) => element
        .AncestorsAndSelf()
        .Select(e => e.Attribute(XmlSpace))
        .OfType<XAttribute>()
        .FirstOrDefault()?.Value == "preserve";

    private static readonly XName XmlSpace = XNamespace.Xml + "space";
}
