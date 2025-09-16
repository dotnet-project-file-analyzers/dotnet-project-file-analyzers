namespace DotNetProjectFile.Analyzers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class SdkAnalyzerAttribute(
    ProjectLanguage language,
    params ProjectLanguage[] otherLanguages) : Attribute
{
    public ImmutableArray<ProjectLanguage> Languages { get; } = otherLanguages.Prepend(language).Distinct().ToImmutableArray();
}
