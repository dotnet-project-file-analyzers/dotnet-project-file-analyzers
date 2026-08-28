using Nullable = DotNetProjectFile.MsBuild.Nullable;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.EnableNullabilityFSharp" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableNullabilityFSharp() : MsBuildProjectFileAnalyzer(Rule.EnableNullabilityFSharp)
{
    public override ImmutableArray<Language> ApplicableLanguages => Languages.FSharp;

    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var prop = context.File.Property<Nullable>();

        if (prop is null || prop.Value is not Nullable.Kind.Enabled)
        {
            context.ReportDiagnostic(Descriptor, (Node?)prop ?? context.File);
        }
    }
}
