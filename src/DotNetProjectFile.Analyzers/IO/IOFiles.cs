#pragma warning disable RS1035 // Do not use APIs banned for analyzers

using System.IO;
using Iterator = System.Collections.Generic.IEnumerator<string>;

namespace DotNetProjectFile.IO;

public sealed class IOFiles(string root, IOMatch[] ignoreDirs) : IEnumerator<FileRef>, IEnumerable<FileRef>
{
    public static readonly IOMatch[] DefaultIgnores =
    [
        new IOMatches.LocalIs("bin"),
        new IOMatches.LocalIs("obj"),
        new IOMatches.LocalIs(".vs"),
        new IOMatches.LocalIs(".git"),
        new IOMatches.LocalIs(".nuget"),
        new IOMatches.LocalIs(".sonar"),
        new IOMatches.LocalIs("TestResults"),
        new IOMatches.LocalIs("node_modules"),
    ];

    private readonly IOMatch[] IgnoreDirs = ignoreDirs;

    private readonly int Offset = root.Length + 1;

    private readonly Stack<Enumerators> Iterators = new([Enumerators.New(root)]);

    /// <inheritdoc />
    public FileRef Current
    {
        get
        {
            var iterator = Iterators.Peek();
            return new FileRef(iterator.File, Offset);
        }
    }

    /// <inheritdoc />
    object IEnumerator.Current => Current;

    /// <inheritdoc />
    public bool MoveNext()
    {
        while (Iterators.Count > 0)
        {
            var iterator = Iterators.Peek();

            if (MoveNext(iterator.Dirs, IgnoreDirs))
            {
                Iterators.Push(Enumerators.New(iterator.Dir));
            }
            else if (iterator.Files.MoveNext())
            {
                return true;
            }
            else
            {
                Iterators.Pop().Dispose();
            }
        }
        return false;
    }

    private bool MoveNext(Iterator iterator, IOMatch[] ignores)
    {
        while (iterator.MoveNext())
        {
            Chars full = iterator.Current.AsSpan()[Offset..];
            var last = full.LastIndexOf('\\') + 1;
            Chars name = full[last..];

            if (!Ignore(full, name)) return true;
        }
        return false;

        bool Ignore(Chars full, Chars name)
        {
            foreach (var ignore in ignores)
            {
                if (ignore.Matches(full, name))
                {
                    return true;
                }
            }
            return false;
        }
    }

    void IDisposable.Dispose()
    {
        while (Iterators.Count > 0)
        {
            Iterators.Pop().Dispose();
        }
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    [DoesNotReturn]
    public void Reset() => throw new NotSupportedException();

    /// <inheritdoc />
    public IEnumerator<FileRef> GetEnumerator() => this;

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => this;

    private readonly struct Enumerators(Iterator dirs, Iterator files) : IDisposable
    {
        public readonly Iterator Dirs = dirs;
        public readonly Iterator Files = files;

        public string Dir => Dirs.Current;

        public string File => Files.Current;

        public static Enumerators New(string root) => new(
            Directory.EnumerateDirectories(root).GetEnumerator(),
            Directory.EnumerateFiles(root).GetEnumerator());

        public void Dispose()
        {
            Dirs.Dispose();
            Files.Dispose();
        }
    }
}
