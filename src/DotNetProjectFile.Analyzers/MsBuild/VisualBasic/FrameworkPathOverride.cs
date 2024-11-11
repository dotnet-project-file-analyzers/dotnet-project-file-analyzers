namespace DotNetProjectFile.MsBuild.VisualBasic;

/// <summary>
/// Specifies the location of mscorlib.dll and microsoft.visualbasic.dll. This
/// parameter is equivalent to the /sdkpath switch of the vbc.exe compiler.
/// </summary>
/// <remarks>Visual Basic only.</remarks>
public sealed class FrameworkPathOverride(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project) { }
