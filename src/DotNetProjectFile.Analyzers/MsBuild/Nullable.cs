namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Enable nullable context, or nullable warnings.
/// </summary>
/// <remarks>C#, and F# only.</remarks>
public sealed class Nullable(XElement element, Node parent, MsBuildProject project)
    : Node<Nullable.Kind?>(element, parent, project)
{
    public enum Kind
    {
        /// <summary>Nullability analysis disabled.</summary>
        Disabled = 0,

        /// <summary>Nullability analysis enabled.</summary>
        Enabled,

        /// <summary>Nullability analysis enabled.</summary>
        Warnings,

        /// <summary>Maybe null support without warnings.</summary>
        Annotations,
    }
}
