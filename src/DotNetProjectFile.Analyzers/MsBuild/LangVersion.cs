namespace DotNetProjectFile.MsBuild;

public sealed class LangVersion(XElement element, Node parent, MsBuildProject project)
    : Node<LanguageVersion>(element, parent, project)
{ }
