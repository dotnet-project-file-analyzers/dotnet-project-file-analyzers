using DotNetProjectFile.Json;

namespace JSON.JsonFile_specs;

public class Parses
{
    [Test]
    public void Simple_Object()
    {
        var tree = Test.Tree("""{"root": "true"}""");
        var json = JsonFile.Parse(tree);

        var obj = (JsonObject)json.Value!;
        var prop = obj.Properties.Single();
        prop.Key!.Text.Should().Be(@"""root""");
        var val = (JsonString)prop.Value!;
        val.Text.Should().Be(@"""true""");

        json.GetDiagnostics().Should().HaveNoIssues();
    }

    [Test]
    public void All_Types()
    {
        var tree = Test.Tree("""
            {
                "str": "hello",
                "num": -12.34e5,
                "bool_t": true,
                "bool_f": false,
                "null_v": null,
                "arr": [1, "two"],
                "obj": {}
            }
            """);
        var json = JsonFile.Parse(tree);

        var obj = (JsonObject)json.Value!;
        obj.Properties.Should().HaveCount(7);

        var propStr = obj.Properties.ElementAt(0);
        propStr.Key!.Text.Should().Be(@"""str""");
        ((JsonString)propStr.Value!).Text.Should().Be(@"""hello""");

        var propNum = obj.Properties.ElementAt(1);
        propNum.Key!.Text.Should().Be(@"""num""");
        ((JsonNumber)propNum.Value!).Text.Should().Be("-12.34e5");

        var propBoolT = obj.Properties.ElementAt(2);
        propBoolT.Key!.Text.Should().Be(@"""bool_t""");
        propBoolT.Value.Should().BeOfType<JsonTrue>();

        var propBoolF = obj.Properties.ElementAt(3);
        propBoolF.Key!.Text.Should().Be(@"""bool_f""");
        propBoolF.Value.Should().BeOfType<JsonFalse>();

        var propNull = obj.Properties.ElementAt(4);
        propNull.Key!.Text.Should().Be(@"""null_v""");
        propNull.Value.Should().BeOfType<JsonNull>();

        var propArr = obj.Properties.ElementAt(5);
        propArr.Key!.Text.Should().Be(@"""arr""");
        var arr = (JsonArray)propArr.Value!;
        arr.Items.Should().HaveCount(2);
        ((JsonNumber)arr.Items.ElementAt(0)).Text.Should().Be("1");
        ((JsonString)arr.Items.ElementAt(1)).Text.Should().Be(@"""two""");

        var propObj = obj.Properties.ElementAt(6);
        propObj.Key!.Text.Should().Be(@"""obj""");
        propObj.Value.Should().BeOfType<JsonObject>();

        json.GetDiagnostics().Should().HaveNoIssues();
    }
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
