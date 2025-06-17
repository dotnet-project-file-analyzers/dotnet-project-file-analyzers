namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.SymbolPackageFormatSNupkgRequiresDebugTypePortable"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class SymbolPackageFormatSNupkgRequiresDebugTypePortable()
    : MsBuildProjectFileAnalyzer(Rule.SymbolPackageFormatSNupkgRequiresDebugTypePortable)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.Property<SymbolPackageFormat>() is { Value: SymbolPackageFormat.Kind.snupkg } format)
        {
            var debugType = context.File.Property<DebugType>();

            if (debugType?.Value is not DebugType.Kind.portable)
            {
                context.ReportDiagnostic(Descriptor, (XmlAnalysisNode?)debugType ?? format);
            }
        }
    }
}
