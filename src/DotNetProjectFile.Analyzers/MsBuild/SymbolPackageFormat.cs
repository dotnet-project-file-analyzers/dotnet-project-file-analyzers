namespace DotNetProjectFile.MsBuild;

public sealed class SymbolPackageFormat(XElement element, Node parent, MsBuildProject project)
    : Node<SymbolPackageFormat.Kind?>(element, parent, project)
{
    public enum Kind
    {
        snupkg,
    }
}
