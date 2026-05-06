using DotNetProjectFile.IO;
using DotNetProjectFile.MsBuild;

namespace MS_Build_ConditionEvaluator_specs;

internal static class Fixture
{
    public static readonly PropertyRegistry Registry = new(
        IOFile.Parse(@"C:\repo\src\Lib\Lib.csproj"),
        IOFile.Parse(@"C:\repo\src\App\App.csproj"));

    public static readonly PropertyRegistry RegistryWithUserDefined = new(
        IOFile.Parse(@"C:\repo\src\Lib\Lib.csproj"),
        IOFile.Parse(@"C:\repo\src\App\App.csproj"),
        name => name switch
        {
            "Configuration" => "Debug",
            "Platform" => "AnyCPU",
            "TargetFramework" => "net10.0",
            "IsTestProject" => "true",
            _ => null,
        });
}

public class Returns_null_for_invalid_input
{
    [Test]
    public void Null_input() => ConditionEvaluator.Evaluate(null, Fixture.Registry).Should().BeNull();

    [Test]
    public void Empty_input() => ConditionEvaluator.Evaluate(string.Empty, Fixture.Registry).Should().BeNull();

    [Test]
    public void Whitespace_only() => ConditionEvaluator.Evaluate("   \t  ", Fixture.Registry).Should().BeNull();

    [Test]
    public void Malformed_unterminated_string()
        => ConditionEvaluator.Evaluate("'unterminated", Fixture.Registry).Should().BeNull();

    [Test]
    public void Bare_value_without_comparison()
        => ConditionEvaluator.Evaluate("'just-a-value'", Fixture.Registry).Should().BeNull();
}

public class Equality_comparisons
{
    [Test]
    public void Two_equal_string_literals_returns_true()
        => ConditionEvaluator.Evaluate("'foo' == 'foo'", Fixture.Registry).Should().BeTrue();

    [Test]
    public void Two_different_string_literals_returns_false()
        => ConditionEvaluator.Evaluate("'foo' == 'bar'", Fixture.Registry).Should().BeFalse();

    [Test]
    public void Equality_is_case_insensitive_matching_MSBuild_semantics()
        => ConditionEvaluator.Evaluate("'Foo' == 'FOO'", Fixture.Registry).Should().BeTrue();

    [Test]
    public void Property_equals_literal_when_resolved_value_matches()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' == 'Debug'", Fixture.RegistryWithUserDefined)
            .Should().BeTrue();

    [Test]
    public void Property_equals_literal_when_resolved_value_differs()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' == 'Release'", Fixture.RegistryWithUserDefined)
            .Should().BeFalse();

    [Test]
    public void Reserved_property_substitutes()
        => ConditionEvaluator.Evaluate(
            "'$(MSBuildProjectName)' == 'App'", Fixture.Registry)
            .Should().BeTrue();
}

public class Inequality_comparisons
{
    [Test]
    public void Different_strings_yield_true()
        => ConditionEvaluator.Evaluate("'foo' != 'bar'", Fixture.Registry).Should().BeTrue();

    [Test]
    public void Equal_strings_yield_false()
        => ConditionEvaluator.Evaluate("'foo' != 'foo'", Fixture.Registry).Should().BeFalse();

    [Test]
    public void Property_not_equal_to_literal()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' != 'Release'", Fixture.RegistryWithUserDefined)
            .Should().BeTrue();
}

public class Logical_and
{
    [Test]
    public void Both_true_yields_true()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' == 'Debug' And '$(Platform)' == 'AnyCPU'",
            Fixture.RegistryWithUserDefined)
            .Should().BeTrue();

    [Test]
    public void First_false_yields_false()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' == 'Release' And '$(Platform)' == 'AnyCPU'",
            Fixture.RegistryWithUserDefined)
            .Should().BeFalse();

    [Test]
    public void Second_false_yields_false()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' == 'Debug' And '$(Platform)' == 'x64'",
            Fixture.RegistryWithUserDefined)
            .Should().BeFalse();

    [Test]
    public void And_keyword_is_case_insensitive()
        => ConditionEvaluator.Evaluate(
            "'foo' == 'foo' and 'bar' == 'bar'", Fixture.Registry)
            .Should().BeTrue();
}

public class Logical_or
{
    [Test]
    public void Either_true_yields_true()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' == 'Debug' Or '$(Platform)' == 'x64'",
            Fixture.RegistryWithUserDefined)
            .Should().BeTrue();

    [Test]
    public void Both_false_yields_false()
        => ConditionEvaluator.Evaluate(
            "'$(Configuration)' == 'Release' Or '$(Platform)' == 'x64'",
            Fixture.RegistryWithUserDefined)
            .Should().BeFalse();

    [Test]
    public void Or_keyword_is_case_insensitive()
        => ConditionEvaluator.Evaluate("'foo' == 'foo' or 'a' == 'b'", Fixture.Registry)
            .Should().BeTrue();
}

public class Chained_logical_operators
{
    [Test]
    public void Three_And_chained_all_true_yields_true()
        => ConditionEvaluator.Evaluate(
            "'a' == 'a' And 'b' == 'b' And 'c' == 'c'", Fixture.Registry)
            .Should().BeTrue();

    [Test]
    public void Three_And_chained_one_false_yields_false()
        => ConditionEvaluator.Evaluate(
            "'a' == 'a' And 'b' == 'X' And 'c' == 'c'", Fixture.Registry)
            .Should().BeFalse();

    [Test]
    public void Three_Or_chained_one_true_yields_true()
        => ConditionEvaluator.Evaluate(
            "'a' == 'X' Or 'b' == 'X' Or 'c' == 'c'", Fixture.Registry)
            .Should().BeTrue();

    [Test]
    public void And_binds_tighter_than_Or_per_MSBuild_semantics()
    {
        // 'a' == 'X' Or ('b' == 'b' And 'c' == 'X') should be (false Or (true And false)) = false
        ConditionEvaluator.Evaluate(
            "'a' == 'X' Or 'b' == 'b' And 'c' == 'X'", Fixture.Registry)
            .Should().BeFalse();
    }

    [Test]
    public void And_binds_tighter_than_Or_left_disjunct_short_circuits_to_true()
    {
        // 'a' == 'a' Or ('b' == 'X' And 'c' == 'c') should be (true Or (false And true)) = true
        ConditionEvaluator.Evaluate(
            "'a' == 'a' Or 'b' == 'X' And 'c' == 'c'", Fixture.Registry)
            .Should().BeTrue();
    }
}

public class Whitespace_flexibility
{
    [Test]
    public void No_spaces_around_equality_operator_still_evaluates()
        => ConditionEvaluator.Evaluate("'a'=='a'", Fixture.Registry).Should().BeTrue();

    [Test]
    public void Tabs_and_newlines_are_treated_as_whitespace()
        => ConditionEvaluator.Evaluate("'a'\t==\n'a'", Fixture.Registry).Should().BeTrue();

    [Test]
    public void Many_spaces_between_tokens_evaluates()
        => ConditionEvaluator.Evaluate("    'a'    ==    'a'   ", Fixture.Registry).Should().BeTrue();
}

public class Bare_property_reference_as_value
{
    [Test]
    public void Property_reference_outside_quotes_substitutes()
        => ConditionEvaluator.Evaluate(
            "$(Configuration) == 'Debug'", Fixture.RegistryWithUserDefined)
            .Should().BeTrue();

    [Test]
    public void Two_property_references_compared_directly()
    {
        // Both resolve to the same value via the user-defined lookup; should compare equal.
        var registry = new PropertyRegistry(
            IOFile.Parse(@"C:\repo\Lib.csproj"),
            IOFile.Parse(@"C:\repo\App.csproj"),
            name => "shared-value");

        ConditionEvaluator.Evaluate("$(A) == $(B)", registry).Should().BeTrue();
    }
}

public class Parentheses
{
    [Test]
    public void Parens_change_evaluation_order()
        => ConditionEvaluator.Evaluate(
            "('a' == 'a' Or 'b' == 'c') And 'foo' == 'foo'", Fixture.Registry)
            .Should().BeTrue();

    [Test]
    public void Nested_parens_evaluate_correctly()
        => ConditionEvaluator.Evaluate(
            "(('a' == 'a'))", Fixture.Registry)
            .Should().BeTrue();

    [Test]
    public void Mismatched_parens_returns_null()
        => ConditionEvaluator.Evaluate("('a' == 'a'", Fixture.Registry).Should().BeNull();
}

public class Unresolved_property
{
    [Test]
    public void Unresolved_in_string_literal_returns_null()
        => ConditionEvaluator.Evaluate(
            "'$(NotResolvable)' == 'value'", Fixture.Registry)
            .Should().BeNull();

    [Test]
    public void Unresolved_does_not_short_circuit_or_through_unrelated()
        => ConditionEvaluator.Evaluate(
            "'foo' == 'foo' Or '$(NotResolvable)' == 'x'", Fixture.Registry)
            .Should().BeTrue();

    [Test]
    public void And_with_one_null_operand_returns_null()
        => ConditionEvaluator.Evaluate(
            "'foo' == 'foo' And '$(NotResolvable)' == 'x'", Fixture.Registry)
            .Should().BeNull();

    [Test]
    public void Or_with_false_and_null_returns_null()
        => ConditionEvaluator.Evaluate(
            "'foo' == 'bar' Or '$(NotResolvable)' == 'x'", Fixture.Registry)
            .Should().BeNull();

    [Test]
    public void And_short_circuits_on_first_false_even_when_second_unresolved()
        => ConditionEvaluator.Evaluate(
            "'foo' == 'bar' And '$(NotResolvable)' == 'x'", Fixture.Registry)
            .Should().BeFalse();
}

public class Exists_function
{
    [Test]
    public void Exists_for_present_directory_returns_true()
    {
        var temp = System.IO.Path.GetTempPath();
        ConditionEvaluator.Evaluate($"Exists('{temp}')", Fixture.Registry).Should().BeTrue();
    }

    [Test]
    public void Exists_for_missing_path_returns_false()
        => ConditionEvaluator.Evaluate(
            @"Exists('C:\definitely-does-not-exist-12345\nope.txt')", Fixture.Registry)
            .Should().BeFalse();

    [Test]
    public void Exists_with_unresolved_property_returns_null()
        => ConditionEvaluator.Evaluate(
            "Exists('$(NotResolvable)')", Fixture.Registry)
            .Should().BeNull();

    [Test]
    public void Exists_is_case_insensitive()
    {
        var temp = System.IO.Path.GetTempPath();
        ConditionEvaluator.Evaluate($"exists('{temp}')", Fixture.Registry).Should().BeTrue();
    }

    [Test]
    public void Unsupported_function_returns_null()
        => ConditionEvaluator.Evaluate("HasTrailingSlash('foo/')", Fixture.Registry).Should().BeNull();
}

public class Unsupported_syntax
{
    [Test]
    public void Numeric_comparison_returns_null()
        => ConditionEvaluator.Evaluate("'5' < '10'", Fixture.Registry).Should().BeNull();

    [Test]
    public void Negation_returns_null()
        => ConditionEvaluator.Evaluate("!'a' == 'b'", Fixture.Registry).Should().BeNull();

    [Test]
    public void Comma_in_input_does_not_tokenize()
        => ConditionEvaluator.Evaluate("'a', 'b'", Fixture.Registry).Should().BeNull();
}

public class Truncated_or_malformed
{
    [Test]
    public void Truncated_function_call_returns_null()
        => ConditionEvaluator.Evaluate("Exists(", Fixture.Registry).Should().BeNull();

    [Test]
    public void Function_call_with_no_arguments_returns_null()
        => ConditionEvaluator.Evaluate("Exists()", Fixture.Registry).Should().BeNull();

    [Test]
    public void Value_followed_by_non_comparison_operator_returns_null()
        => ConditionEvaluator.Evaluate("'foo' And 'bar'", Fixture.Registry).Should().BeNull();
}
