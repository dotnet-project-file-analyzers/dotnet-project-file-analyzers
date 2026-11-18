using System.IO;

namespace DotNetProjectFile.IO;

/// <summary>Represents an (IO) file.</summary>
/// <remarks>
/// TODO: Merge with <see cref="IOFile"/>.
/// </remarks>
public readonly struct FileRef(string path, int offset = 0)
{
    public readonly string FullName = path;

    public string Name
    {
        get
        {
            var slash = FullName.LastIndexOf('/');
            if (slash is -1) slash = FullName.LastIndexOf('\\');
            return FullName[(slash + 1)..];
        }
    }

    public override string ToString() => FullName[offset..];

    /// <summary>Opens a read-only <see cref="FileStream" />.</summary>
    [Pure]
    public FileStream OpenRead() => new(FullName, FileMode.Open, FileAccess.Read);
}
