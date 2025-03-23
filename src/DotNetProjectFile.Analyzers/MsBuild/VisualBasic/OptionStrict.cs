namespace DotNetProjectFile.MsBuild.VisualBasic;

/// <summary>
/// A boolean value that when set to true, causes the build task to enforce
/// strict type semantics to restrict implicit type conversions. This property
/// is equivalent to the /optionstrict switch of the vbc.exe compiler.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class OptionStrict(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{ }
