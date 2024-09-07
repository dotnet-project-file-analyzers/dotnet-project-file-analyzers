namespace System.Xml.Linq;

internal static class XElementExtensions
{
    public static bool PreservesSpace(this XElement element) => element
        .AncestorsAndSelf()
        .Select(e => e.Attribute(XmlSpace))
        .OfType<XAttribute>()
        .FirstOrDefault()?.Value == "preserve";

    private static readonly XName XmlSpace = XNamespace.Xml + "space";
}
