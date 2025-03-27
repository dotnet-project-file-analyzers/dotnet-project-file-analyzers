namespace DotNetProjectFile.MsBuild;

public sealed class GenerateSBOM(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
