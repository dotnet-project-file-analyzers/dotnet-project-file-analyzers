using System.IO;

namespace DotNetProjectFile.IO;

public sealed class IOFiles(string rootPath, string searchPattern = "*") : IEnumerable<FileInfo>
{
    private readonly DirectoryInfo _root = new DirectoryInfo(rootPath);
    private readonly string _searchPattern = searchPattern;

    public Enumerator GetEnumerator() => new(_root, _searchPattern);

    IEnumerator<FileInfo> IEnumerable<FileInfo>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public sealed class Enumerator(DirectoryInfo root, string searchPattern) : IEnumerator<FileInfo>
    {
        private readonly DirectoryInfo _root = root;
        private readonly string _searchPattern = searchPattern;
        private readonly Stack<IEnumerator<FileSystemInfo>> _iterators = new();
        private bool _isInitialized;

        public FileInfo Current { get; private set; } = null!;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                if (!_root.Exists) return false;
                PushDirectory(_root);
            }

            while (_iterators.Count > 0)
            {
                var currentIterator = _iterators.Peek();

                if (currentIterator.MoveNext())
                {
                    switch (currentIterator.Current)
                    {
                        case FileInfo file:
                            Current = file;
                            return true;

                        case DirectoryInfo subDir:
                            PushDirectory(subDir);
                            break;
                    }
                }
                else
                {
                    // Current directory finished; pop and dispose its OS handle
                    _iterators.Pop().Dispose();
                }
            }

            Current = null!;
            return false;
        }

        private void PushDirectory(DirectoryInfo dir)
        {
            try
            {
                // EnumerateFileSystemInfos yields both FileInfo and DirectoryInfo lazily
                var enumerator = dir.EnumerateFileSystemInfos(_searchPattern).GetEnumerator();
                _iterators.Push(enumerator);
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose()
        {
            DisposeIterators();
            GC.SuppressFinalize(this);
        }

        private void DisposeIterators()
        {
            while (_iterators.Count > 0)
            {
                _iterators.Pop().Dispose();
            }
        }
    }
}
