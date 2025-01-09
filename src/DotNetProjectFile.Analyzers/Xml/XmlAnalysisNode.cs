namespace DotNetProjectFile.Xml;

public interface XmlAnalysisNode
{
    XElement Element { get; }

    XmlLocations Locations { get; }

    string LocalName { get; }

    int Depth { get; }

    IEnumerable<XmlAnalysisNode> Children();
}
