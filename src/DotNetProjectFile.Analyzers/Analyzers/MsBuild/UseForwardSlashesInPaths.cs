namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.UseForwardSlashesInPaths"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseForwardSlashesInPaths() : MsBuildProjectFileAnalyzer(Rule.UseForwardSlashesInPaths)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var prop in context.File
            .DescendantsAndSelf()
            .SelectMany(AsProps)
            .Where(WithBackSlash))
        {
            context.ReportDiagnostic(Descriptor, prop.Node, $"{prop.Node.LocalName} {prop.Property}".Trim());
        }
    }

    private static bool WithBackSlash(Prop prop) => prop.Value?.Contains('\\') == true;

    private static IEnumerable<Prop> AsProps(Node node) => node switch
    {
        Import import => [new(import, nameof(Import.Project), import.Attribute(nameof(import.Project)))],
        DockerfileContext docker => [new(docker, string.Empty, docker.Value)],
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
