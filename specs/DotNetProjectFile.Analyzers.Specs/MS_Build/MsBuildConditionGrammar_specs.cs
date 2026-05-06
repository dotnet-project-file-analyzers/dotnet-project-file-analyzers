using DotNetProjectFile.MsBuild;
using Grammr.Text;

namespace MS_Build_MsBuildConditionGrammar_specs;

public class WhiteSpace_token
{
    [Test]
    public void Matches_single_space()
    {
        var results = MsBuildConditionGrammar.WhiteSpace.Parse(TokenStream.From(" abc"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = " ", Kind = "WhiteSpace" } },
                Success = true,
                Remaining = new { Text = "abc" },
            }
        ]);
    }

    [Test]
    public void Matches_mixed_whitespace()
    {
        var results = MsBuildConditionGrammar.WhiteSpace.Parse(TokenStream.From(" \t\r\nrest"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = " \t\r\n", Kind = "WhiteSpace" } },
                Success = true,
                Remaining = new { Text = "rest" },
            }
        ]);
    }

    [Test]
    public void Does_not_match_empty()
    {
        var results = MsBuildConditionGrammar.WhiteSpace.Parse(TokenStream.From("abc"), new());

        results.Outcome.Should().BeEquivalentTo(new { Success = false, Message = "Expected WhiteSpace." });
    }
}

public class StringLiteral_token
{
    [Test]
    public void Matches_simple_quoted_string()
    {
        var results = MsBuildConditionGrammar.StringLiteral.Parse(TokenStream.From("'Debug' rest"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "'Debug'", Kind = "StringLiteral" } },
                Success = true,
                Remaining = new { Text = " rest" },
            }
        ]);
    }

    [Test]
    public void Matches_empty_quoted_string()
    {
        var results = MsBuildConditionGrammar.StringLiteral.Parse(TokenStream.From("''"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "''", Kind = "StringLiteral" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void Matches_string_with_property_reference_inside()
    {
        var results = MsBuildConditionGrammar.StringLiteral.Parse(TokenStream.From("'$(Configuration)'"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "'$(Configuration)'", Kind = "StringLiteral" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void Does_not_match_unterminated_quote()
    {
        var results = MsBuildConditionGrammar.StringLiteral.Parse(TokenStream.From("'unterminated"), new());

        results.Outcome.Should().BeEquivalentTo(new { Success = false, Message = "Expected StringLiteral." });
    }
}

public class PropertyReference_token
{
    [Test]
    public void Matches_simple_property_reference()
    {
        var results = MsBuildConditionGrammar.PropertyReference.Parse(TokenStream.From("$(Configuration)"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "$(Configuration)", Kind = "PropertyReference" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void Does_not_match_dotted_name()
    {
        var results = MsBuildConditionGrammar.PropertyReference.Parse(TokenStream.From("$(Foo.Bar)"), new());

        results.Outcome.Should().BeEquivalentTo(new { Success = false, Message = "Expected PropertyReference." });
    }

    [Test]
    public void Does_not_match_empty_name()
    {
        var results = MsBuildConditionGrammar.PropertyReference.Parse(TokenStream.From("$()"), new());

        results.Outcome.Should().BeEquivalentTo(new { Success = false, Message = "Expected PropertyReference." });
    }
}

public class Identifier_token
{
    [Test]
    public void Matches_simple_identifier()
    {
        var results = MsBuildConditionGrammar.Identifier.Parse(TokenStream.From("Exists()"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "Exists", Kind = "Identifier" } },
                Success = true,
                Remaining = new { Text = "()" },
            }
        ]);
    }

    [Test]
    public void Matches_logical_operator_keywords()
    {
        var results = MsBuildConditionGrammar.Identifier.Parse(TokenStream.From("And rest"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "And", Kind = "Identifier" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void Does_not_match_starting_with_digit()
    {
        var results = MsBuildConditionGrammar.Identifier.Parse(TokenStream.From("3foo"), new());

        results.Outcome.Should().BeEquivalentTo(new { Success = false, Message = "Expected Identifier." });
    }
}

public class Comparison_operator_tokens
{
    [Test]
    public void EqualEqual_matches()
    {
        var results = MsBuildConditionGrammar.EqualEqual.Parse(TokenStream.From("== 'Debug'"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "==", Kind = "EqualEqual" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void NotEqual_matches()
    {
        var results = MsBuildConditionGrammar.NotEqual.Parse(TokenStream.From("!= 'Release'"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "!=", Kind = "NotEqual" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void LessOrEqual_matches()
    {
        var results = MsBuildConditionGrammar.LessOrEqual.Parse(TokenStream.From("<= 5"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "<=", Kind = "LessOrEqual" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void GreaterOrEqual_matches()
    {
        var results = MsBuildConditionGrammar.GreaterOrEqual.Parse(TokenStream.From(">= 1"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = ">=", Kind = "GreaterOrEqual" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void Less_matches_single_char()
    {
        var results = MsBuildConditionGrammar.Less.Parse(TokenStream.From("<rest"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "<", Kind = "Less" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void Greater_matches_single_char()
    {
        var results = MsBuildConditionGrammar.Greater.Parse(TokenStream.From(">rest"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = ">", Kind = "Greater" } },
                Success = true,
            }
        ]);
    }
}

public class Punctuation_tokens
{
    [Test]
    public void LeftParenthesis_matches()
    {
        var results = MsBuildConditionGrammar.LeftParenthesis.Parse(TokenStream.From("(arg"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "(", Kind = "LeftParenthesis" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void RightParenthesis_matches()
    {
        var results = MsBuildConditionGrammar.RightParenthesis.Parse(TokenStream.From(") rest"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = ")", Kind = "RightParenthesis" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void Comma_matches()
    {
        var results = MsBuildConditionGrammar.Comma.Parse(TokenStream.From(",arg"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = ",", Kind = "Comma" } },
                Success = true,
            }
        ]);
    }

    [Test]
    public void ExclamationMark_matches()
    {
        var results = MsBuildConditionGrammar.ExclamationMark.Parse(TokenStream.From("!cond"), new());

        results.Should().BeEquivalentTo([
            new
            {
                Stream = new[] { new { Text = "!", Kind = "ExclamationMark" } },
                Success = true,
            }
        ]);
    }
}
