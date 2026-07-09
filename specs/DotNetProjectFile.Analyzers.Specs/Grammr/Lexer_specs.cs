using Grammr.Lexers;
using static Grammr.Lexers.Shared;

namespace Specs.Lexer_specs;

public class EndOfLine
{
    [TestCase("\n", "\n")]
    [TestCase("\r\n", "\r\n")]
    [TestCase("\nNext line", "\n")]
    [TestCase("\r\nNext line", "\r\n")]
    public void Matches(string src, string match)
        => eol.Should().Match(match, from: src);

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("Some text ")]
    public void Does_not_match(string src)
        => eol.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => eol.ToString().Should().Be("[eol]");
}

public class EndOfStream
{
    [TestCase("", "")]
    public void Matches(string src, string match)
        => eos.Should().Match(match, from: src);

    [TestCase("\r\n")]
    [TestCase(" ")]
    [TestCase("Some text ")]
    public void Does_not_match(string src)
        => eos.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => eos.ToString().Should().Be("[eos]");
}

public class Whitespace
{
    [TestCase("\t", "\t")]
    [TestCase(" ", " ")]
    [TestCase("    ", "    ")]
    [TestCase(" \t   ", " \t   ")]
    public void Matches(string src, string match)
        => ws.Should().Match(match, from: src);

    [TestCase("\r\n")]
    [TestCase("")]
    [TestCase("Some text")]
    public void Does_not_match(string src)
        => ws.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => ws.ToString().Should().Be("match(IsWhitespace)");
}

public class Char
{
    private readonly Lexer lexer = ch('x');

    [TestCase("x", "x")]
    [TestCase("x as first", "x")]
    public void Match(string src, string match)
        => lexer.Should().Match(match, from: src);

    [TestCase("")]
    [TestCase("y as first")]
    public void Does_not_match(string src)
        => lexer.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => lexer.ToString().Should().Be("ch(x)");
}

public class String
{
    private readonly Lexer lexer = str("grammr");

    [TestCase("grammr", "grammr")]
    [TestCase("grammrasfirstword", "grammr")]
    [TestCase("grammr as first word", "grammr")]
    public void Parses(string src, string match)
        => lexer.Should().Match(match, from: src);

    [TestCase("")]
    [TestCase("not starting with grammr")]
    public void Does_not_match(string src)
        => lexer.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => lexer.ToString().Should().Be("str(grammr)");
}

public class Match
{
    private readonly Lexer lexer = match(IsDigit);
    private static bool IsDigit(char c, int _) => c is >= '0' and <= '9';

    [TestCase("1 is a number", "1")]
    [TestCase("42!", "42")]
    [TestCase("17", "17")]
    public void Parses(string src, string match)
        => lexer.Should().Match(match, from: src);

    [TestCase("")]
    [TestCase("not starting with 42")]
    public void Does_not_match(string src)
        => lexer.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => lexer.ToString().Should().Be("match(IsDigit)");
}

public class Regex
{
    private readonly Lexer lexer = reg("[x-z]+");

    [TestCase("x", "x")]
    [TestCase("x as first", "x")]
    [TestCase("y as first", "y")]
    [TestCase("z as first", "z")]
    [TestCase("xxxyzzy as first", "xxxyzzy")]
    public void Parses(string src, string match)
        => lexer.Should().Match(match, from: src);

    [TestCase("")]
    [TestCase("not starting with x or y or z")]
    public void Does_not_match(string src)
        => lexer.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => lexer.ToString().Should().Be("regex([x-z]+)");
}

public class Line
{
    [TestCase("some text\n", /*...*/ "some text")]
    [TestCase("some text\r\n", /*.*/ "some text")]
    [TestCase("some text \n", /*..*/ "some text ")]
    public void Parses(string src, string match)
        => line().Should().Match(match, from: src);

    [TestCase("")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    public void Does_not_match(string src)
        => line().Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => line().ToString().Should().Be("line()");
}

public class LineComment
{
    private readonly Lexer lexer = line_comment("//");

    [TestCase("//", "//")]
    [TestCase("// some comment\n", "// some comment")]
    [TestCase("// some comment\r\n", "// some comment")]
    [TestCase("// some comment \n", "// some comment ")]
    public void Parses(string src, string match)
        => lexer.Should().Match(match, from: src);

    [TestCase("")]
    [TestCase("/ not starting with //")]
    public void Does_not_match(string src)
        => lexer.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => lexer.ToString().Should().Be("line_comment(//)");
}

public class Optional
{
    private readonly Lexer lexer = str("abc").optional;

    [TestCase("", "")]
    [TestCase("abc", "abc")]
    [TestCase("abcefdg", "abc")]
    [TestCase("abc ", "abc")]
    [TestCase("xyz", "")]
    public void Matches(string src, string match)
        => lexer.Should().Match(match, from: src);

    [Test]
    public void Has_debug_display() => lexer.ToString().Should().Be("str(abc)?");

    [Test]
    public void Optional_lexer_matches_empty_string()
        => ((object)lexer.optional).Should().BeSameAs(lexer);
}

public class Choice
{
    private readonly Lexer lexer = ch('x') | str("42") | ch('?');

    [TestCase("x42", "x")]
    [TestCase("x42 as first", "x")]
    [TestCase("42", "42")]
    [TestCase("?", "?")]
    public void Parses(string src, string match)
        => lexer.Should().Match(match, from: src);

    [TestCase("")]
    [TestCase("wxyz as first")]
    public void Does_not_match(string src)
        => lexer.Should().NotMatch(src);

    [Test]
    public void Has_debug_display() => lexer.ToString().Should().Be("(ch(x) | str(42) | ch(?))");
}
