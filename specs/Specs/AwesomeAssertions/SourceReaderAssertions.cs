using Grammr;

namespace Specs.TestTools;

internal sealed class SourceReaderAssertions(SourceReader subject)
{
    public SourceReaderInfo Subject { get; } = new(subject);

    public TokenStream Stream => Subject.Stream;

    public AndConstraint<SourceReaderAssertions> Be(string span, string because = "", params object[] becauseArgs)
    {
        Subject.Span.Should().Be(span, because, becauseArgs);

        return new(this);
    }
}

internal sealed class SourceReaderInfo
{
    public SourceReaderInfo(SourceReader subject)
    {
        Source = subject.Source;
        Stream = subject.Stream;
        Span = subject.Span.ToString();
    }

    public Source Source { get; }

    public TokenStream Stream { get; }

    public string Span { get; }
}