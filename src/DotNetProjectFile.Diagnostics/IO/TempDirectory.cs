using System.IO;

namespace DotNetProjectFile.Diagnostics.IO;

/// <summary>Represents a directory that lives during the lifetime of its scope.</summary>
/// <remarks>
/// Should always been used with a using statement.
///
/// <code>
/// using(var directory = new TemporaryDirectory())
/// {
///     // Do stuff.
/// }
/// </code>
/// </remarks>
internal sealed class TemporaryDirectory : IDisposable
{
    /// <summary>Initializes a new instance of the <see cref="TemporaryDirectory" /> class.</summary>
    public TemporaryDirectory()
    {
        Root = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Uuid.NewUuid().ToString()));
        Root.Create();
    }

    /// <summary>Gets the full name of the directory.</summary>
    public string FullName => Root.FullName;

    /// <summary>Gets all files, from both the top- and subdirectories.</summary>
    [Pure]
    public IEnumerable<FileInfo> GetDlls() => Root.EnumerateFiles("*.dll", SearchOption.AllDirectories);

    /// <summary>Represents the temporary directory as <see cref="string" />.</summary>
    [Pure]
    public override string ToString() => Root.ToString();

    /// <summary>The underlying <see cref="DirectoryInfo" />.</summary>
    private readonly DirectoryInfo Root;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool IsDisposed;

    /// <summary>Disposes the temporary directory by deleting it and its content.</summary>
    public void Dispose()
    {
        if (!IsDisposed)
        {
            Root.Delete(true);
            IsDisposed = true;
        }
    }
}
