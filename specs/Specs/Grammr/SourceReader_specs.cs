#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute', but not here: we're testing the other path
using Grammr;
using System.Text.RegularExpressions;
using static Grammr.Lexers.Shared;

namespace Specs.SourceReader_specs;

public class Consumes
{
    [Test]
    public void Length()
    {
        var reader = new SourceReader("abcde");
        reader.Cons(3).Should().BeTrue();
        reader
            .Should().Be("de")
            .And.Stream.Should().BeEmpty();
    }

    [Test]
    public void Matching_lexer()
    {
        var reader = new SourceReader("abcde");
        reader.Cons(str("abc")).Should().BeTrue();
        reader
            .Should().Be("de")
            .And.Stream.Should().BeEmpty();
    }

    [Test]
    public void Non_matching_lexer()
    {
        var reader = new SourceReader("abcde");
        reader.Cons(str("xyz")).Should().BeFalse();
        reader
            .Should().Be("abcde")
            .And.Stream.Should().BeEmpty();
    }
}

public class Takes
{
    [Test]
    public void Matching_lexer()
    {
        var lexer = str("abc", "ABC-kind");
        var reader = new SourceReader("abcde");

        reader.Take(lexer).Should()
            .Match(new("abc", "ABC-kind"));

        reader
            .Should().Be("de")
            .And.Stream.Should().BeEmpty();
    }

    [Test]
    public void Non_matching_lexer()
    {
        var lexer = str("abc", "ABC-kind");
        var reader = new SourceReader("123de");

        reader.Take(lexer)
            .Should().NotMatch();

        reader
            .Should().Be("123de")
            .And.Stream.Should().BeEmpty();
    }

    [Test]
    public void Matching_choice_lexer()
    {
        var lexer = str("xyz", "XYZ-kind") | str("abc", "ABC-kind");
        var reader = new SourceReader("abcde");

        reader.Take(lexer).Should()
            .Match(new("abc", "ABC-kind"));

        reader
            .Should().Be("de")
            .And.Stream.Should().BeEmpty();
    }

    [Test]
    public void Non_matching_choice_lexer()
    {
        var lexer = str("xyz", "XYZ-kind") | str("abc", "ABC-kind");
        var reader = new SourceReader("123de");

        reader.Take(lexer)
            .Should().NotMatch();

        reader
            .Should().Be("123de")
            .And.Stream.Should().BeEmpty();
    }
}

public class Emits
{
    [Test]
    public void Matching_lexer()
    {
        var lexer = str("abc", "ABC-kind");
        var reader = new SourceReader("abcde");

        reader.Emit(lexer).Should()
            .Match(new("abc", "ABC-kind"));

        reader
            .Should().Be("de")
            .And.Stream.Should().ContainSingle();
    }

    [Test]
    public void Non_matching_lexer()
    {
        var lexer = str("abc", "ABC-kind");
        var reader = new SourceReader("123de");

        reader.Emit(lexer)
            .Should().NotMatch();

        reader
            .Should().Be("123de")
            .And.Stream.Should().BeEmpty();
    }
}

public class Keeps
{
    [Test]
    public void Matching_lexer()
    {
        var lexer = str("abc", "ABC-kind");
        var reader = new SourceReader("abcde");

        reader.Keep(lexer).Should().BeTrue();

        reader
            .Should().Be("de")
            .And.Stream.Should().ContainSingle();
    }

    [Test]
    public void Non_matching_lexer()
    {
        var lexer = str("abc", "ABC-kind");
        var reader = new SourceReader("123de");

        reader.Keep(lexer).Should().BeFalse();

        reader
            .Should().Be("123de")
            .And.Stream.Should().BeEmpty();
    }
}

public class Matches
{
    [Test]
    public void Regex_from_start()
        => new SourceReader("abcd")
            .Match(new("^a"))
            .Should().BeEquivalentTo(new { Success = true, Index = 0, Length = 1 });

    [Test]
    public void Regex_from_later()
    {
        var reader = new SourceReader("abcd");
        reader.Cons(2).Should().BeTrue();

        var match = reader.Match(new("^."));
        match.Should().BeEquivalentTo(new { Success = true, Index = 2, Length = 1 });
    }

    [Test]
    public void Regex_start_at()
    {
        var pattern = new Regex("^42");
        var str = "42abc42test";

        pattern.Match(str, 0).Should().BeEquivalentTo(new { Success = true, Index = 0, Length = 2 });
        pattern.Match(str, 5, str.Length - 5).Should().BeEquivalentTo(new { Success = true, Index = 5, Length = 2 });
    }
}
