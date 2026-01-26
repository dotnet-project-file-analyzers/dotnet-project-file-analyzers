namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.UpdateShouldChangeState"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UpdateShouldChangeState() : MsBuildProjectFileAnalyzer(Rule.UpdateShouldChangeState)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var action in context.File.Project.ItemGroups
            .Children<Node>(WithoutChanges))
        {
            context.ReportDiagnostic(Descriptor, action);
        }
    }

    /// <summary>
    /// True when the <see cref="BuildAction"/> only contains an Update attribute.
    /// </summary>
    private static bool WithoutChanges(Node action)
        => action.Attribute("Update") is { }
        && action.Element.Attributes().Skip(1).None()
        && action.Element.Elements().None();
}
