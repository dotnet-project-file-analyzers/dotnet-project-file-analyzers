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
        Disable = 0,

        /// <summary>Nullability analysis enabled.</summary>
        Enable,

        /// <summary>Nullability analysis enabled without warning on assignments.</summary>
        Warnings,

        /// <summary>Maybe null support without warnings.</summary>
        Annotations,
    }
}
