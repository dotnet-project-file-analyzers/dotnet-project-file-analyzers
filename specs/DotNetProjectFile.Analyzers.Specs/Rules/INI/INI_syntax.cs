using DotNetProjectFile.Analyzers.Ini;

namespace Rules.INI.INI_syntax;

public class Reports
{
    [Test]
    public void errors() => new IniSyntaxAnalyzer()
        .ForProject("IniSyntaxErrors.cs")
        .HasIssues(
            //new Issue("Proj4000", "Key value pair expected.").WithSpan(02, 00, 02, 10),
            //new Issue("Proj4000", "Key value pair expected.").WithSpan(03, 00, 03, 27),
            //new Issue("Proj4000", "Key value pair expected.").WithSpan(04, 00, 04, 17),
            //new Issue("Proj4000", "Key value pair expected.").WithSpan(10, 00, 10, 26),
            new Issue("Proj4001", "[ is unexpected." /*.*/).WithSpan(15, 01, 15, 02),
            new Issue("Proj4002", "] is unexpected." /*.*/).WithSpan(12, 01, 12, 02),
            new Issue("Proj4003", "] is missing." /*....*/).WithSpan(02, 10, 03, 00));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new IniSyntaxAnalyzer()
        .ForProject(project)
        .HasNoIssues();
}

