namespace DotNetProjectFile.MsBuild;

public sealed class EditorConfgFiles(XElement element, Node parent, MsBuildProject project)
    : BuildAction(element, parent, project)
{ }
