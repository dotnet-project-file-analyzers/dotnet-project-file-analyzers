namespace DotNetProjectFile.MsBuild;

public sealed class TargetFramework(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{
    public static readonly string net9_0 = "net9.0";
    public static readonly string net10_0 = "net10.0";
}
