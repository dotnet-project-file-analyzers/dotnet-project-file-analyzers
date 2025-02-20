using DotNetProjectFile;
using System.Reflection;

namespace Extensions.String_distance_specs;

public class DamerauLevenshtein
{
    [TestCase(null, null, 0)]
    [TestCase("", "", 0)]
    [TestCase("", null, 0)]
    [TestCase("Test", "Tes", 1)]
    [TestCase("Test", "Tets", 1)]
    [TestCase("FooBar", "foobar", 2)]
    [TestCase("foobaz", "foobarbar", 4)]
    [TestCase("foobaz", "foobarbaz", 3)]
    [TestCase("foobaz", "foobazbar", 3)]
    public void Should_be(string? str1, string? str2, int expectedDistance)
    {
        Distance(str1, str2).Should().Be(expectedDistance);
        Distance(str2, str1).Should().Be(expectedDistance);
    }

    private static readonly Lazy<MethodInfo> method = new(GetMethod);

    private static MethodInfo GetMethod()
        => typeof(Rule).Assembly.GetTypes()
        .First(t => t.FullName == "System.StringExtensions")
        .GetMethods()
        .First(m => m.Name == "DamerauLevenshteinDistanceTo");

    private static int Distance(string? a, string? b)
        => (int)method.Value.Invoke(null, [a, b])!;
}
