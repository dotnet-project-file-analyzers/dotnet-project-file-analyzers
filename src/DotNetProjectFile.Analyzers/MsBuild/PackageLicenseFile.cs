namespace DotNetProjectFile.MsBuild;

public sealed class PackageLicenseFile : Node
{
    public PackageLicenseFile(XElement element, Node parent, Project project) : base(element, parent, project) { }

    public string? Value => Convert<string?>(Element.Value);
}
