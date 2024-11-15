namespace System.IO;

public static class DotNetProjectFileAnalyzersFileInfoExtensions
{
    [Pure]
    public static IDisposable Lock(this FileInfo file) => new FileLock(file);

    private sealed class FileLock(FileInfo File) : IDisposable
    {
        private readonly FileStream Stream = new(File.FullName, new FileStreamOptions
        {
            Share = FileShare.None,
            Mode = FileMode.Open,
            Access = FileAccess.Read,
        });

        /// <inheritdoc />
        public void Dispose()
        {
            if (!Disposed)
            {
                Stream.Dispose();
                Disposed = true;
            }
        }
        private bool Disposed;
    }
}
