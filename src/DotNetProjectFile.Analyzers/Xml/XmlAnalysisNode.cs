namespace DotNetProjectFile.Xml;

public interface XmlAnalysisNode
{
    XmlPositions Positions { get; }

    string LocalName { get; }

    int Depth { get; }

    IEnumerable<XmlAnalysisNode> Children();
}
