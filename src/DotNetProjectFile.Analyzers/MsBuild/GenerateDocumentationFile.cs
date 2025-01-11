namespace DotNetProjectFile.MsBuild;

public sealed class GenerateDocumentationFile(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
