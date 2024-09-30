namespace DotNetProjectFile.IO.Globbing;

internal sealed class Unparsable(string text) : Segement
{
    public string Text { get; } = text;

    public override int MinLength => throw new NotSupportedException();

    public override int MaxLength => throw new NotSupportedException();

    public override string ToString() => $"<<{Text}>>";
}
