namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Specifies the file format of the output file. This parameter can have one
/// of the following values:
/// - Library: Creates a code library. (Default value.)
/// - Exe:    Creates a console application.
/// - Module: Creates a module.
/// - Winexe: Creates a Windows-based program.
///
/// For C# and Visual Basic, this property is equivalent to the /target switch.
/// Disable inferencing by setting DisableWinExeOutputInference to true.
/// </summary>
/// <remarks>All.</remarks>
public sealed class OutputType(XElement element, Node parent, MsBuildProject project)
    : Node<OutputType.Kind?>(element, parent, project)
{
    public enum Kind
    {
        Library,
        Exe,
        Module,
        WinExe,
    }
}
