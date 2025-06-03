namespace DotNetProjectFile.Slnx;

public sealed class Folder(XElement element, Node parent, Solution solution)
    : Node(element, parent, solution) { }
