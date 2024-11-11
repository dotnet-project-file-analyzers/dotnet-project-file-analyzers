namespace DotNetProjectFile.MsBuild.VisualBasic;

/// <summary>
/// Specifies how string comparisons are made. Valid values are "binary" or
/// "text." This property is equivalent to the /optioncompare compiler switch of vbc.exe.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class OptionCompare(XElement element, Node? parent, MsBuildProject? project)
    : Node<bool?>(element, parent, project) { }
