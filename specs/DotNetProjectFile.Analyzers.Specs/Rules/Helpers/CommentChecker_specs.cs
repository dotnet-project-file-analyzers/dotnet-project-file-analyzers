using DotNetProjectFile.Analyzers.Helpers;

namespace Rules.Helpers.CommentChecker_specs;

public class Detects_commented_out_XML
{
    [TestCase(@" PackageReference Include=""System.Text.Json"" Version=""9.0.0"" /")]
    public void with_replaced_tags(string comment)
        => CommentChecker.ContainsXml(comment).Should().NotBeNull();

    [TestCase(@"Was <Import Project=""../props/common.prop""/> ")]
    [TestCase(@"<Import Project=""../props/common.prop""/> ")]
    [TestCase(@"<Import Project=""../props/common.prop"" /> ")]
    [TestCase(@"Somewhere <br> in my text")]
    public void containing_XML_tags(string comment)
        => CommentChecker.ContainsXml(comment).Should().NotBeNull();
}
