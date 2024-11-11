namespace DotNetProjectFile.MsBuild;

/// <summary>
/// A boolean value that when set to true, requires explicit declaration of
/// variables in the source code. This property is equivalent to the
/// /optionexplicit compiler switch.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class OptionExplicit(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
