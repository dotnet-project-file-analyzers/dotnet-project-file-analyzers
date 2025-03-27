namespace DotNetProjectFile.MsBuild.CSharp;

/// <summary>
/// Enable nullable context, or nullable warnings.
/// </summary>
/// <remarks>C# only.</remarks>
public sealed class Nullable(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{ }
