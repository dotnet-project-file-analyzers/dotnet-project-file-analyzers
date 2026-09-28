namespace DotNetProjectFile.MsBuild;

public sealed class AppDesignerFolder(XElement element, Node parent, MsBuildProject project)
    : Node<IOFile?>(element, parent, project);
