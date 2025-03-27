namespace DotNetProjectFile.MsBuild;

public sealed class Using(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Alias => Attribute();

    public bool? Static => Convert<bool?>(Attribute());

    public UsingType Type => (Static ?? false, Alias) switch
    {
        (false, null) => UsingType.Default,
        (false, _) => UsingType.Alias,
        (true, null) => UsingType.Static,
        /* (true, _)*/
        _ => UsingType.StaticAlias,
    };
}
