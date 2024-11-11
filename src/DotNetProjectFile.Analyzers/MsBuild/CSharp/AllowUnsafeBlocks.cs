namespace DotNetProjectFile.MsBuild.CSharp;

/// <summary>
/// The AllowUnsafeBlocks compiler option allows code that uses the unsafe
/// keyword to compile. The default value for this option is false, meaning
/// unsafe code isn't allowed.
/// </summary>
/// <remarks>C# only.</remarks>
public sealed class AllowUnsafeBlocks(XElement element, Node parent, MsBuildProject project)
    : Node<bool?>(element, parent, project) { }
