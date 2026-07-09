using DotNetProjectFile.Analyzers.Helpers;

namespace Rules.ToDoChecker_specs;

public class Is_issue
{
    [Test]
    public void starts_with() => ToDoChecker.IsIssue("TODO: update implementation").Should().Be("TODO");

    [TestCase("TODO")]
    [TestCase("todo")]
    [TestCase("TODOs")]
    [TestCase("TO-DOs")]
    [TestCase("TODO's")]
    [TestCase("TO DO")]
    [TestCase("TO-DO")]
    [TestCase("FIX-me")]
    [TestCase("FIX me")]
    [TestCase("FIXME")]
    public void equals(string txt) => ToDoChecker.IsIssue(txt).Should().Be(txt);

    [Test]
    public void contains() => ToDoChecker.IsIssue(@"
        // TODO: adjust code
        ")
        .Should().Be("TODO");
}

public class No_issue
{
    [TestCase("mastodonts")]
    [TestCase("todolella")]
    public void part_of_word(string txt) => ToDoChecker.IsIssue(txt).Should().BeNull();
}
