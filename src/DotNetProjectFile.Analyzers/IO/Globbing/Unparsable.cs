namespace DotNetProjectFile.IO.Globbing;

internal sealed class Unparsable(string text) : Segement
{
    public string Text { get; } = text;

    /// <inheritdoc />
    public override int MinLength => throw new NotSupportedException();

    /// <inheritdoc />
    public override int MaxLength => throw new NotSupportedException();

    /// <inheritdoc />
    public override bool IsParseble => false;

    public override string ToString() => $"<<{Text}>>";
}
