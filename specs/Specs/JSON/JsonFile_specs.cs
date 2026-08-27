using DotNetProjectFile.Json;
using System.Diagnostics.CodeAnalysis;

namespace JSON.JsonFile_specs;

public class Parses
{
    private static TNode Json<TNode>([StringSyntax(StringSyntaxAttribute.Json)] string json)
        where TNode : JsonValue
    {
        var file = JsonFile.Parse(Test.Tree(json));
        file.GetDiagnostics().Should().HaveNoIssues();
        return (TNode)file.Value!;
    }

    [Test]
    public void valid_JSON_without_issues()
        => JsonFile.Parse(Test.Tree("""
        {
          "str": "hello",
          "num": -12.34e5,
          "bool_t": true,
          "bool_f": false,
          "null_v": null,
          "arr": [1, "two"],
          "obj": {
            "prop": 42,
            "arr": [17, 42]
          }
        }
        """)).GetDiagnostics().Should().HaveNoIssues();

    [Test]
    public void Array_nodes()
        => Json<JsonArray>("[17, 42]").Items
        .Should().HaveCount(2)
        .And.AllBeOfType<JsonNumber>();

    [Test]
    public void Object_nodes()
        => Json<JsonObject>("""
            {
                "first": 17,
                "second": true
            }
            """)
        .Properties.Should().HaveCount(2)
        .And.AllBeOfType<JsonProperty>();

    [Test]
    public void String_nodes()
        => Json<JsonString>("\"Hello, world!\"")
        .Text.Should().Be("Hello, world!");

    [TestCase("0")]
    [TestCase("-13")]
    [TestCase("0.42")]
    [TestCase("-0.456")]
    [TestCase("42234")]
    [TestCase("1.456e5")]
    [TestCase("14.56e+5")]
    [TestCase("14.56e-5")]
    public void Number_nodes(string json)
       => Json<JsonNumber>(json).Text.Should().HaveLength(json.Length);

    [Test]
    public void True_nodes()
        => Json<JsonTrue>("true").Should().NotBeNull();

    [Test]
    public void False_nodes()
        => Json<JsonFalse>("false").Should().NotBeNull();

    [Test]
    public void Null_nodes()
        => Json<JsonNull>("null").Should().NotBeNull();
}

public class Parses_with_issues
{
    [Test]
    public void Object_with_unparsable_characters()
    {
        var tree = Test.Tree("""{"root": "true" % }""");
        var json = JsonFile.Parse(tree);

        json.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj6000", "'%' is unexpected"));
    }

    [Test]
    public void Array_with_unparsable_characters()
    {
        var tree = Test.Tree("""[1, 2, &]""");
        var json = JsonFile.Parse(tree);

        json.GetDiagnostics()
            .Should().HaveIssue(Issue.ERR("Proj6000", "'&' is unexpected"));
    }
}
