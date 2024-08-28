namespace DotNetProjectFile.Xml;

public interface XmlAnalysisNode
{
    XmlPositions Positions { get; }

    public string LocalName { get; }

    public int Depth { get; }

    IEnumerable<XmlAnalysisNode> Children();
}
