namespace DotNetProjectFile.MsBuild;

public sealed class EnableUnsafeBinaryFormatterSerialization(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project);
