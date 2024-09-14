using Specs.TestTools;
using static DotNetProjectFile.Parsing.Grammar;
using Token = FluentAssertions.Token;

namespace Parsing_Grammar;

public class Matches
{
    [Theory]
    [TestCase("a")]
    [TestCase("b")]
    [TestCase("c")]
    public void Or(string s)
    {
        var grammar = ch('a') | ch('b') | ch('c');
        grammar.Parse(Source.Text(s))
            .Should()
            .HaveTokenized(Token.New(0, s));
    }

    [Test]
    public void Not()
    {
        var grammar = ~ch('a') & str("bb");
        grammar.Parse(Source.Text("bb"))
            .Should()
            .HaveTokenized(Token.New(0, "bb"));
    }

    [Test]
    public void Option_none()
    {
        var grammar = ch('a').Option & str("bc");
        grammar.Parse(Source.Text("bc"))
            .Should()
            .HaveTokenized(
                Token.New(0, "bc"));
    }

    [Test]
    public void Option_once()
    {
        var grammar = ch('a').Option & str("bc");
        grammar.Parse(Source.Text("abc"))
            .Should()
            .HaveTokenized(
                Token.New(0, "a"),
                Token.New(1, "bc"));
    }

    [Test]
    public void Predicate()
    {
        var grammar = match(char.IsDigit) & str("bc");
        grammar.Parse(Source.Text("0123456789bc"))
            .Should()
            .HaveTokenized(
                Token.New(0, "0123456789"),
                Token.New(10, "bc"));
    }

    [Test]
    public void Repeat()
    {
        var grammar = ch('a').Plus & str("bc");
        grammar.Parse(Source.Text("aaaabc"))
            .Should()
            .HaveTokenized(
                Token.New(0, "a"),
                Token.New(1, "a"),
                Token.New(2, "a"),
                Token.New(3, "a"),
                Token.New(4, "bc"));
    }

    [Test]
    public void Repeat_on_zero_match()
    {
        var grammar = ch('a') & regex(".*").Option.Plus;
        grammar.Parse(Source.Text("a"))
            .Should()
            .HaveTokenized(Token.New(0, "a"));
    }

    [Test]
    public void Repeat_till_end()
    {
        var grammar = ch('a').Star;
        grammar.Parse(Source.Text("aaaa"))
            .Should()
            .HaveTokenized(
                Token.New(0, "a"),
                Token.New(1, "a"),
                Token.New(2, "a"),
                Token.New(3, "a"));
    }

    [Test]
    public void Start_with_char()
    {
        var grammar = ch('a');
        grammar.Parse(Source.Text("a"))
            .Should()
            .HaveTokenized(Token.New(0, "a"));
    }

    [Test]
    public void Start_with_string()
    {
        var grammar = str("abc");
        grammar.Parse(Source.Text("abc"))
            .Should()
            .HaveTokenized(Token.New(0, "abc"));
    }

    [Test]
    public void End_of_File()
    {
        var grammar = ch('a') & eof;
        grammar.Parse(Source.Text("a"))
            .Should()
            .HaveTokenized(Token.New(0, "a"));
    }
}

public class Does_not_match
{
    [Test]
    public void Or()
    {
        var grammar = ch('a') | ch('b') | ch('c');
        grammar.Parse(Source.Text("d"))
            .Should().NotHaveTokenized();
    }

    [Test]
    public void Not()
    {
        var grammar = ~ch('a') & str("aa");
        grammar.Parse(Source.Text("aa"))
            .Should()
            .NotHaveTokenized();
    }

    [Test]
    public void Predicate()
    {
        var grammar = match(char.IsDigit);
        grammar.Parse(Source.Text("a12"))
            .Should()
            .NotHaveTokenized();
    }

    [Theory]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(6)]
    public void Repeat(int count)
    {
        var grammar = ch('a').Repeat(2, 4);
        grammar.Parse(Source.Text(new string('a', count)))
            .Should().NotHaveTokenized();
    }

    [Test]
    public void Start_with_char()
    {
        var grammar = ch('a');
        grammar.Parse(Source.Text("b"))
            .Should()
            .NotHaveTokenized();
    }

    [Theory]
    [TestCase("")]
    [TestCase("ab")]
    [TestCase("abd")]
    public void Start_with_string(string s)
    {
        var grammar = str("abc");
        grammar.Parse(Source.Text(s))
            .Should()
            .NotHaveTokenized();
    }

    [Test]
    public void End_of_File()
    {
        var grammar = ch('a') & eof & ch('a');
        grammar.Parse(Source.Text("aa"))
            .Should()
            .NotHaveTokenized();
    }
}
