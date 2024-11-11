namespace DotNetProjectFile.MsBuild.VisualBasic;

/// <summary>
/// A boolean value that indicates whether the Visual Basic runtime (Microsoft.VisualBasic.dll)
/// should be included as a reference in the project.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class NoVBRuntimeReference(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
