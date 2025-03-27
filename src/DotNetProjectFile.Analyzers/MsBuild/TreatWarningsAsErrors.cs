namespace DotNetProjectFile.MsBuild;

public sealed class TreatWarningsAsErrors(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
