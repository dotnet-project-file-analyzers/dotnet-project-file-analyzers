namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.AvoidChangingCompilerTools" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidChangingCompilerTools() : MsBuildProjectFileAnalyzer(Rule.AvoidChangingCompilerTools)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var property in context.File.PropertyGroups.Children<Node>(IsCompilerTool))
        {
            context.ReportDiagnostic(Descriptor, property, property.LocalName);
        }
    }

    private static bool IsCompilerTool(Node node) => node
        is CscToolExe
        or CscToolPath
        or VbcToolExe
        or VbcToolPath
        or DotnetFscCompilerPath;
}
