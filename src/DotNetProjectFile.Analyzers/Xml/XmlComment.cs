namespace DotNetProjectFile.Xml;

public sealed class XmlComment(XComment comment, ProjectFile project) : XmlAnalysisNode
{
    /// <inheritdoc />
    public XElement Element { get; } = comment.Parent;

    public XComment Comment { get; } = comment;

    public ProjectFile Project { get; } = project;

    /// <inheritdoc />
    public XmlLocations Locations { get; } = XmlPositions.New(comment).Locations(project);

    /// <inheritdoc cref="XComment.Value" />
    public string Text => Comment.Value;

    /// <inheritdoc />
    public string LocalName => string.Empty;

    /// <inheritdoc />
    public int Depth => 1;

    /// <inheritdoc />
    [Pure]
    public IEnumerable<XmlAnalysisNode> Children() => [];
}
