namespace DotNetProjectFile.MsBuild.CSharp;

/// <summary>
/// The CheckForOverflowUnderflow option controls the default overflow-checking
/// context that defines the program behavior if integer arithmetic overflows.
/// </summary>
/// <remarks>C# only.</remarks>
public sealed class CheckForOverflowUnderflow(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project)
{ }
