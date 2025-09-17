namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{
    /// <summary>Target framework net9.0.</summary>
    public static readonly string net9_0 = "net9.0";

    /// <summary>Target framework net10.0.</summary>
    public static readonly string net10_0 = "net10.0";
}
