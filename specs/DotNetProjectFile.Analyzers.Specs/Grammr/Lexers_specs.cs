using Grammr.Text;
using Grammr;
using Token = Grammr.Token;

namespace Grammr_Lexers_specs;

public class Tokenize : Grammar
{
    private static readonly Token EndOfLine = eol();
    private static readonly Token C = ch('C');
    private static readonly Token ABC = str("ABC");
    private static readonly Token WhiteSpace = match(char.IsWhiteSpace);
    private static readonly Token Digits = regex("[0-9]+");
    private static readonly Token OptionalSpace = regex(@" *");

    [Test]
    public void chars()
    {
        var results = C.Parse(TokenStream.From("CDE"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new []{ new { Text = "C", Kind = "C" } },
                Success = true,
                Remaining = new { Text = "DE" },
            }]);
    }

    [Test]
    public void strings()
    {
        var results = ABC.Parse(TokenStream.From("ABCDEF"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new []{ new { Text = "ABC", Kind = "ABC" } },
                Success = true,
                Remaining = new { Text = "DEF" },
            }]);
    }

    [Test]
    public void predications()
    {
        var results = WhiteSpace.Parse(TokenStream.From(" \t\r\nABC"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new []{ new { Text = " \t\r\n", Kind = "WhiteSpace" } },
                Success = true,
                Remaining = new { Text = "ABC" },
            }]);
    }

    [Test]
    public void regular_expressions()
    {
        var results = Digits.Parse(TokenStream.From("42d"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new []{ new { Text = "42", Kind = "Digits" } },
                Success = true,
                Remaining = new { Text = "d" },
            }]);
    }


    [Test]
    public void optional_regular_expressions()
    {
        var results = OptionalSpace.Parse(TokenStream.From("42d"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = Array.Empty<object>(),
                Success = true,
                Remaining = new { Text = "42d" },
            }]);
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public void end_of_lines(string start)
    {
        var results = EndOfLine.Parse(TokenStream.From($"{start}Hello"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new []{ new { Text = start, Kind = "EndOfLine" } },
                Success = true,
                Remaining = new { Text = "Hello" },
            }]);
    }

    [Test]
    public void end_of_file()
    {
        var parser = ABC & eof;

        var results = parser.Parse(TokenStream.From("ABC"), new());
        results.Outcome.Should().BeEquivalentTo(
            new
            {
                Stream = new[] { new { Text = "ABC", Kind = "ABC" }, new { Text = string.Empty, Kind = "EOF" } },
                Success = true,
                Remaining = new { Text = "" },
            });
    }
}

public class Does_not_match : Grammar
{
    private static readonly Token C = ch('C');
    private static readonly Token ABC = str("ABC");
    private static readonly Token WhiteSpace = match(char.IsWhiteSpace);
    private static readonly Token Digits = regex("[0-9]+");

    [Test]
    public void chars()
    {
        var results = C.Parse(TokenStream.From("ABCDE"), new());

        results.Outcome.Should().BeEquivalentTo(
            new
            {
                Stream = Array.Empty<object>(),
                Success = false,
                Remaining = new { Text = "ABCDE" },
                Message = "Expected C.",
            });
    }

    [Test]
    public void strings()
    {
        var results = ABC.Parse(TokenStream.From("BCDEF"), new());

        results.Outcome.Should().BeEquivalentTo(
            new
            {
                Stream = Array.Empty<object>(),
                Success = false,
                Remaining = new { Text = "BCDEF" },
                Message = "Expected ABC.",
            });
    }

    [Test]
    public void predications()
    {
        var results = WhiteSpace.Parse(TokenStream.From("ABC\nCDE"), new());

        results.Outcome.Should().BeEquivalentTo(
            new
            {
                Stream = Array.Empty<object>(),
                Success = false,
                Remaining = new { Text = "ABC\nCDE" },
                Message = "Expected WhiteSpace.",
            });
    }

    [Test]
    public void regular_expressions()
    {
        var results = Digits.Parse(TokenStream.From("Test42d"), new());

        results.Outcome.Should().BeEquivalentTo(
            new
            {

                Stream = Array.Empty<object>(),
                Success = false,
                Remaining = new { Text = "Test42d" },
                Message = "Expected Digits.",
            });
    }

    [Test]
    public void end_of_file()
    {
        var results = eof.Parse(TokenStream.From("Test42d"), new());

        results.Outcome.Should().BeEquivalentTo(
            new
            {

                Stream = Array.Empty<object>(),
                Success = false,
                Remaining = new { Text = "Test42d" },
                Message = "Expected EOF.",
            });
    }
}

public class Regular_expressions : Grammar
{
    [Test]
    public void start_of_string_is_added_when_not_specified()
    {
        var token = regex(@"\d+");
        token.Parse(TokenStream.From("ABC123"), new()).Outcome.Success.Should().BeFalse();
    }

    [Test]
    public void start_of_string_is_not_added_when_specified()
    {
        var token = regex(@"^\d+");
        token.Parse(TokenStream.From("123"), new()).Outcome.Success.Should().BeTrue();
    }
}
