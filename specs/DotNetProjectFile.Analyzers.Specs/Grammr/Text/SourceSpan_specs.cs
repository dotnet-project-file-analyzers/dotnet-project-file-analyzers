using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr_Text.SourceSpan_specs;

public class IndexOf
{
    [Test]
    public void null_if_not_found()
    {
        SourceSpan span = Source.From("ABC");
        span.IndexOf('D').Should().BeNull();
    }

    [Test]
    public void null_if_only_found_before()
    {
        SourceSpan span = Source.From("ABC");
        span++;
        span.IndexOf('A').Should().BeNull();
    }

    [Test]
    public void null_if_only_found_after()
    {
        SourceSpan span = Source.From("ABC");
        span = span.Trim(new(0, 2));
        span.IndexOf('C').Should().BeNull();
    }

    [Test]
    public void TextSpan_of_length_1()
    {
        SourceSpan span = Source.From("ABCEFGHIJKLMNOPQR");
        span++;
        var text = span.IndexOf('Q');
        text.Should().Be(new TextSpan(15, 1));
    }
}

public class Match
{
    [Test]
    public void null_if_not_found()
    {
        SourceSpan span = Source.From("ABC");
        span.Match(@"\d").Should().BeNull();
    }

    [Test]
    public void null_if_only_found_before()
    {
        SourceSpan span = Source.From("1BC");
        span++;
        span.Match(@"\d").Should().BeNull();
    }

    [Test]
    public void null_if_only_found_after()
    {
        SourceSpan span = Source.From("AB3");
        span = span.Trim(new(0, 2));
        span.Match(@"\d").Should().BeNull();
    }

    [Test]
    public void TextSpan_of_length_matches()
    {
        SourceSpan span = Source.From("ABCEFGHI123MNOPQR");
        span++;
        var text = span.Match(@"\d+");
        text.Should().Be(new TextSpan(8, 3));
    }

    [Test]
    public void TextSpan_of_length_zero_on_successful_empty_match()
    {
        SourceSpan span = Source.From("ABCEFGHI123MNOPQR");
        span++;
        var text = span.Match(@"\s*");
        text.Should().Be(new TextSpan(1, 0));
    }
}

public class StartsWith
{
    public class Chars
    {
        [Test]
        public void null_if_false()
        {
            SourceSpan span = Source.From("ABC");
            span.StartsWith('D').Should().BeNull();
        }

        [Test]
        public void null_if_character_before_span()
        {
            SourceSpan span = Source.From("ABC");
            span++;
            span.StartsWith('A').Should().BeNull();
        }

        [Test]
        public void TextSpan_of_length_1()
        {
            SourceSpan span = Source.From("ABCEFGHIJKLMNOPQR");
            span++;
            var text = span.StartsWith('B');
            text.Should().Be(new TextSpan(1, 1));
        }
    }

    public class Strings
    {
        [Test]
        public void null_if_false()
        {
            SourceSpan span = Source.From("ABC");
            span.StartsWith("D").Should().BeNull();
        }

        [Test]
        public void null_if_too_long()
        {
            SourceSpan span = Source.From("ABC");
            span.StartsWith("ABCD").Should().BeNull();
        }

        [TestCase("A")]
        [TestCase("AB")]
        public void null_if_character_before_span(string value)
        {
            SourceSpan span = Source.From("ABC");
            span++;
            span.StartsWith(value).Should().BeNull();
        }

        [TestCase("B", 1)]
        [TestCase("BC", 2)]
        [TestCase("BCD", 3)]
        public void TextSpan_of_match(string value, int length)
        {
            SourceSpan span = Source.From("ABCDEFGHIJKLMNOPQR");
            span++;
            var text = span.StartsWith(value);
            text.Should().Be(new TextSpan(1, length));
        }
    }
}

public class Predicate
{
    [Test]
    public void null_if_not_found()
    {
        SourceSpan span = Source.From("ABC");
        span.Predicate(char.IsAsciiDigit).Should().BeNull();
    }

    [Test]
    public void null_if_only_found_before()
    {
        SourceSpan span = Source.From("1BC");
        span++;
        span.Predicate(char.IsAsciiDigit).Should().BeNull();
    }

    [Test]
    public void null_if_only_found_after()
    {
        SourceSpan span = Source.From("AB3");
        span = span.Trim(new(0, 2));
        span.Predicate(char.IsAsciiDigit).Should().BeNull();
    }

    [TestCase("1234")]
    [TestCase("1234ABC")]
    public void TextSpan_of_length_matches(string source)
    {
        SourceSpan span = Source.From(source);
        span++;
        var text = span.Predicate(char.IsAsciiDigit);
        text.Should().Be(new TextSpan(1, 3));
    }
}

public class First
{
    [TestCase('A', 0)]
    [TestCase('B', 1)]
    [TestCase('C', 2)]
    public void returns_first(char first, int skip)
    {
        SourceSpan span = Source.From("ABCD");
        span = span.Skip(skip);
        span.First.Should().Be(first);
    }

    [TestCase]
    public void throws_for_empty()
    {
        SourceSpan span = Source.From(string.Empty);
        span.Invoking(s => s.First).Should().Throw<IndexOutOfRangeException>();
    }
}
