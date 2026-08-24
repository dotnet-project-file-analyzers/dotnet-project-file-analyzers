namespace DotNetProjectFile.Slnx;

public sealed class File(XElement element, Node parent, SolutionFile solution)
    : Node(element, parent, solution)
{
    public string? Path => Attribute();

    public IOFile FullPath
        => Convert<IOFile>(IOPath.IsFullyQualified(Path)
        ? Path
        : System.IO.Path.Combine(Solution.Path.Directory.ToString(), Path));
}
