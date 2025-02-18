using DotNetProjectFile.Licensing;
using System.Collections.Immutable;

namespace Licensing.Design_specs;

public class Is_defined
{
    private static readonly ImmutableArray<string> DeprecatedExpressions
        = Licenses.All
        .OfType<SingleLicense>()
        .SelectMany(l => l.Deprecated)
        .Distinct()
        .ToImmutableArray();

    private static readonly ImmutableArray<string> CompatibleExpressions
        = Licenses.All
        .OfType<CopyLeftLicense>()
        .SelectMany(l => l.Compatibilities)
        .Distinct()
        .ToImmutableArray();

    [TestCaseSource(nameof(DeprecatedExpressions))]
    public void Deprecated(string expressionName)
    {
        Licenses.FromExpression(expressionName).Should().NotBe(Licenses.Unknown);
    }

    [TestCaseSource(nameof(CompatibleExpressions))]
    public void Compatibilities(string expressionName)
    {
        Licenses.FromExpression(expressionName).Should().NotBe(Licenses.Unknown);
    }
}
