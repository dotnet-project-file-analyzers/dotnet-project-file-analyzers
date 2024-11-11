namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Specifies the verbosity of the Visual Basic compiler's output. Valid values
/// are:
/// - Quiet
/// - Normal (the default value)
/// - Verbose.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class VbcVerbosity(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project) { }
