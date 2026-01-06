namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.SymbolPackageFormatSNupkgRequiresDebugTypePortable"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SymbolPackageFormatSNupkgSetup() : MsBuildProjectFileAnalyzer(
    Rule.SymbolPackageFormatSNupkgRequiresDebugTypePortable,
    Rule.SymbolPackageFormatSNupkgRequiresIncludeSymbolsToBeEnabled)
{
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.Property<SymbolPackageFormat>() is { Value: SymbolPackageFormat.Kind.snupkg } format)
        {
            var debugType = context.File.Property<DebugType>();
            var includeSymbols = context.File.Property<IncludeSymbols>();

            if (debugType?.Value is not DebugType.Kind.portable)
            {
                context.ReportDiagnostic(Rule.SymbolPackageFormatSNupkgRequiresDebugTypePortable, (XmlAnalysisNode?)debugType ?? format);
            }

            if (includeSymbols?.Value is not true)
            {
                context.ReportDiagnostic(Rule.SymbolPackageFormatSNupkgRequiresIncludeSymbolsToBeEnabled, (XmlAnalysisNode?)includeSymbols ?? format);
            }
        }
    }
}
