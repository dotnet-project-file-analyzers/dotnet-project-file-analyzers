using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers.NuGetConfig;

/// <summary>Implements <see cref="Rule.NuGet.InjectCredentials"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class InjectCredentials() : NuGetConfigFileAnalyzer(Rule.NuGet.InjectCredentials)
{
    /// <inheritdoc />
    protected override void Register(NuGetConfigFileAnalysisContext context)
    {
        foreach (var credentials in context.File.Children.OfType<PackageSourceCredentials>())
        {
            foreach (var section in credentials.Children)
            {
                foreach (var add in section.Children.OfType<Add>().Where(a => "ClearTextPassword".IsMatch(a.Key)))
                {
                    if (add.Value is { Length: > 2 } value && !IsPlaceholder(value))
                    {
                        var att = AttributesPositions.New(add.Element.Attribute("value"), context.File.Text);
                        context.ReportDiagnostic(Descriptor, context.File.GetLocation(att.Value));
                    }
                }
            }
        }
    }

    private static bool IsPlaceholder(string value)
        => value[0] is '%'
        && value[^1] is '%'
        && value[1..^2].All(IsPlaceholder);

    private static bool IsPlaceholder(char c)
        => c is >= 'a' and <= 'z'
        || c is >= 'A' and <= 'Z'
        || c is >= '0' and <= '9'
        || c is '_';
}
