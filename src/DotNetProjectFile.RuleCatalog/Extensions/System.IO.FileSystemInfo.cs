namespace System.IO;

internal static class FileInfoExtensions
{
    extension(DirectoryInfo dir)
    {
        /// <summary>Gets all files, from both the top- and subdirectories.</summary>
        [Pure]
        public IEnumerable<FileInfo> GetDlls() => dir.EnumerateFiles("*.dll", SearchOption.AllDirectories);
    }

    extension(FileInfo file)
    {
        public string NameWithoutExtension => Path.GetFileNameWithoutExtension(file.Name);
    }
}
