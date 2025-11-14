namespace DotNetProjectFile.Slnx;

public sealed class Folder(XElement element, Node parent, SolutionFile solution)
    : Node(element, parent, solution) { }
