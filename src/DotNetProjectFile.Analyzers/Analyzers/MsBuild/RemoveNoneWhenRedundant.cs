namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveNoneWhenRedundant"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveNoneWhenRedundant() : MsBuildProjectFileAnalyzer(Rule.RemoveNoneWhenRedundant)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var documentationFile = context.File.Property<DocumentationFile>();

        foreach (var none in context.File.ItemGroups
            .Children<None>(n => n.Remove.Any() && !Except(n)))
        {
            context.ReportDiagnostic(Descriptor, none, string.Concat(';', none.Remove));
        }

        bool Except(None none)
            => documentationFile is { } doc
            && none.Remove.Any(remove => remove == doc.Value);
    }
}
