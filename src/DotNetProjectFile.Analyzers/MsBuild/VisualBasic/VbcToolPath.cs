namespace DotNetProjectFile.MsBuild.VisualBasic;

/// <summary>
/// An optional path that indicates another location for vbc.exe when the
/// current version of vbc.exe is overridden.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class VbcToolPath(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project) { }
