using Nullable = DotNetProjectFile.MsBuild.Nullable;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.EnableNullabilityCSharp" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableNullabilityCSharp() : MsBuildProjectFileAnalyzer(Rule.EnableNullabilityCSharp)
{
    public override ImmutableArray<Language> ApplicableLanguages => Languages.CSharp;

    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var prop = context.File.Property<Nullable>();

        if (!Enabled(prop?.Value))
        {
            context.ReportDiagnostic(Descriptor, (Node?)prop ?? context.File);
        }
    }

    private static bool Enabled(Nullable.Kind? kind) => kind
        is Nullable.Kind.Enabled
        or Nullable.Kind.Annotations
        or Nullable.Kind.Warnings;
}
