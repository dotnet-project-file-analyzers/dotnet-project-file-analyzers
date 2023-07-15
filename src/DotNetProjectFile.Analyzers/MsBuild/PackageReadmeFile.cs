namespace DotNetProjectFile.MsBuild;

public sealed class PackageReadmeFile : Node
{
    public PackageReadmeFile(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Value => Convert<string?>(Element.Value);
}
