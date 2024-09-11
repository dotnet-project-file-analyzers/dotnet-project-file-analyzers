namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseForwardSlashesInPaths() : MsBuildProjectFileAnalyzer(Rule.UseForwardSlashesInPaths)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var prop in context.Project
            .DescendantsAndSelf()
            .SelectMany(AsProps)
            .Where(WithBackSlash))
        {
            context.ReportDiagnostic(Description, prop.Node, prop.Node.LocalName, prop.Property);
        }
    }

    private static bool WithBackSlash(Prop prop) => prop.Value?.Contains('\\') == true;

    private static IEnumerable<Prop> AsProps(Node node) => node switch
    {
        Import import => [new Prop(import, nameof(Import.Project), import.Attribute(nameof(import.Project)))],
        _ => AsPropsGeneric(node),
    };

    private static IEnumerable<Prop> AsPropsGeneric(Node node)
    {
        if (node.Attribute("Include") is { } include) yield return new(node, "Include", include);
        if (node.Attribute("Update") is { } update) yield return new(node, "Update", update);
        if (node.Attribute("Remove") is { } remove) yield return new(node, "Remove", remove);
        if (node.Attribute("Link") is { } link) yield return new(node, "Link", link);
    }

    private sealed record Prop(Node Node, string Property, string? Value);
}
