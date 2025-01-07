namespace DotNetProjectFile.Xml;

public sealed class XmlComment(XComment comment) : XmlAnalysisNode
{
    /// <inheritdoc />
    public XElement Element { get; } = comment.Parent;

    public XComment Comment { get; } = comment;

    /// <inheritdoc />
    public XmlPositions Positions { get; } = XmlPositions.New(comment);

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
