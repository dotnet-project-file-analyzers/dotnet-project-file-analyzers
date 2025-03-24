namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableGenerateDocumentationFile() : MsBuildProjectFileAnalyzer(Rule.GenerateDocumentationFile)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsPackable() 
            || context.File.IsTestProject()
            || context.File.IsDevelopmentDependency()) return;

        var generateDocumentationFile = context.File.Property<GenerateDocumentationFile>();
        var documentationFile = context.File.Property<DocumentationFile>();

        if (generateDocumentationFile?.Value == true)
        {
            // <GenerateDocumentationFile>true<GenerateDocumentationFile/>
            return;
        }
        if (generateDocumentationFile?.Value == false)
        {
            // <GenerateDocumentationFile>false<GenerateDocumentationFile/>
            context.ReportDiagnostic(Descriptor, generateDocumentationFile);
        }
        else
        {
            if (documentationFile is null)
            {
                // Neither.
                context.ReportDiagnostic(Descriptor, context.File);
            }
            else if (documentationFile.Value is null || documentationFile.Value.Length == 0)
            {
                // <DocumentationFile><DocumentationFile/>
                context.ReportDiagnostic(Descriptor, documentationFile);
            }
            else
            {
                // <DocumentationFile>../../..<DocumentationFile/>
            }
        }
    }
}
