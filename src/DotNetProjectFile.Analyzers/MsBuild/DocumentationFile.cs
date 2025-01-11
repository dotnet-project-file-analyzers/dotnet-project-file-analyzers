namespace DotNetProjectFile.MsBuild;

public sealed class DocumentationFile(XElement element, Node parent, MsBuildProject project)
    : Node<string>(element, parent, project)
{ }
