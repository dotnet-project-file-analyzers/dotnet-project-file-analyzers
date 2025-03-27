namespace DotNetProjectFile.MsBuild.VisualBasic;

/// <summary>
/// A boolean value that when set to true, enables type inference of variables.
/// This property is equivalent to the /optioninfer compiler switch.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class OptionInfer(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{ }
