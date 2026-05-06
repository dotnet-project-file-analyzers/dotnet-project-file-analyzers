namespace DotNetProjectFile.Xml;

/// <summary>Represents a node in an XML document being analyzed.</summary>
public interface XmlAnalysisNode
{
    /// <summary>The parent node.</summary>
    XmlAnalysisNode? Parent { get; }

    /// <summary>The linked <see cref="XElement"/>.</summary>
    XElement Element { get; }

    /// <summary>The <see cref="XmlLocations"/> of the node.</summary>
    XmlLocations Locations { get; }

    /// <summary>The local name.</summary>
    string LocalName { get; }

    /// <summary>The zero-based depth of the node in the hierarchy.</summary>
    int Depth { get; }

    /// <summary>The child nodes.</summary>
    IEnumerable<XmlAnalysisNode> Children();
}
