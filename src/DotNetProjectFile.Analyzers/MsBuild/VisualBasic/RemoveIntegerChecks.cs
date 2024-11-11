namespace DotNetProjectFile.MsBuild.VisualBasic;

/// <summary>
/// A boolean value that indicates whether to disable integer overflow error
/// checks. The default value is false. This property is equivalent to the
/// /removeintchecks switch of the vbc.exe compiler.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class RemoveIntegerChecks(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
