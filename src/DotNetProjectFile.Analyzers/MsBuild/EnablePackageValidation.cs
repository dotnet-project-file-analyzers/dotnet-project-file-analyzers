namespace DotNetProjectFile.MsBuild;

public sealed class EnablePackageValidation(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
