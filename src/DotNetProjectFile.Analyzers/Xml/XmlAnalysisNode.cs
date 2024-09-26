namespace DotNetProjectFile.Xml;

public interface XmlAnalysisNode
{
    XElement Element { get; }

    XmlPositions Positions { get; }

    string LocalName { get; }

    int Depth { get; }

    IEnumerable<XmlAnalysisNode> Children();
}
