namespace DotNetProjectFile.Caching;

/// <summary>
/// A limited cache (default capacity of 8), to prevent re-processing project
/// files, by bounding the result to the current compilation.
/// </summary>
public sealed class CompilationCache<T> where T : class
{
    private readonly object locker = new();
    private readonly Compilation[] Keys;
    private readonly T[] Values;
    private int Next;

    public CompilationCache(int capacity = 16)
    {
        Capacity = capacity;
        Keys = new Compilation[capacity];
        Values = new T[capacity];
    }

    public int Capacity { get; }

    public T Get(Compilation compilation, Func<T> create)
    {
        lock (locker)
        {
            if (Array.IndexOf(Keys, compilation) is { } index && index != -1)
            {
                return Values[index];
            }
            else
            {
                var value = create();

                Keys[Next] = compilation;
                Values[Next] = value;
                Next = (Next + 1) % Capacity;

                return value;
            }
        }
    }
}
