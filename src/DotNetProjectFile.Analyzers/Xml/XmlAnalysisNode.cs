namespace DotNetProjectFile.Xml;

public interface XmlAnalysisNode
{
    XElement Element { get; }

    XmlPositions Positions { get; }

    string LocalName { get; }

    int Depth { get; }

    ProjectFile Project { get; }

    IEnumerable<XmlAnalysisNode> Children();
}
