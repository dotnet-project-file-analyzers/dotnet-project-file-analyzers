using System.Collections.Frozen;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class LanguageVersionShouldBeExplicitVersionNumber() : MsBuildProjectFileAnalyzer(Rule.LanguageVersionShouldBeExplicitVersionNumber)
{
    private static readonly FrozenSet<string> ForbiddenVersions = new[]
    {
        "latest",
        "default",
        "latestMajor",
        "preview",
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.Property<LangVersion>() is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
        else if (!IsExplicitVersion(node.Value))
        {
            context.ReportDiagnostic(Descriptor, node);
        }
    }

    private static bool IsExplicitVersion(string? versionString)
    {
        if (string.IsNullOrWhiteSpace(versionString))
        {
            return false;
        }

        versionString = versionString?.Trim()!;

        return !ForbiddenVersions.Contains(versionString);
    }
}
