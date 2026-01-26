namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.PreferLatestLanguageVersion"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class PreferLatestLanguageVersion() : MsBuildProjectFileAnalyzer(Rule.PreferLatestLanguageVersion)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    public override ImmutableArray<Language> ApplicableLanguages { get; } = [Language.CSharp, Language.VisualBasic, Language.FSharp];

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Options.GetSdkVersion() is not { IsNone: false } sdkVersion ||
            context.File.Property<LangVersion>()?.Value is not { IsNone: true } langVersion) return;

        var advised = context.CompilationLanguage switch
        {
            var l when l == Language.CSharp => CSharp(sdkVersion),
            var l when l == Language.FSharp => FSharp(sdkVersion),
            var l when l == Language.VisualBasic => VisualBasic(sdkVersion),
            _ => LanguageVersion.None,
        };

        if (advised > langVersion)
        {
            context.ReportDiagnostic(Descriptor, context.File.Property<LangVersion>()!, advised, sdkVersion);
        }
    }

    private static LanguageVersion CSharp(SdkVersion sdk) => sdk switch
    {
        _ when sdk >= SdkVersion.NET10 => LanguageVersion.CS14,
        _ when sdk >= SdkVersion.NET9 => LanguageVersion.CS13,
        _ when sdk >= SdkVersion.NET8 => LanguageVersion.CS12,
        _ when sdk >= SdkVersion.NET7 => LanguageVersion.CS11,
        _ when sdk >= SdkVersion.NET6 => LanguageVersion.CS10,
        _ => new(1),
    };

    private static LanguageVersion FSharp(SdkVersion sdk) => sdk switch
    {
        _ when sdk >= SdkVersion.NET10 => LanguageVersion.FS10,
        _ when sdk >= SdkVersion.NET9 => LanguageVersion.FS9,
        _ when sdk >= SdkVersion.NET8 => LanguageVersion.FS8,
        _ when sdk >= SdkVersion.NET7 => LanguageVersion.FS7,
        _ when sdk >= SdkVersion.NET6 => LanguageVersion.FS6,
        _ => new(1),
    };

    private static LanguageVersion VisualBasic(SdkVersion sdk) => sdk switch
    {
        _ when sdk >= SdkVersion.NET9 => LanguageVersion.VB17_13,
        _ when sdk >= SdkVersion.NET6 => LanguageVersion.VB16_9,
        _ => new(1),
    };
}
