namespace DotNetProjectFile.Navigation;

[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Length = {Length}")]
internal class ProjectTrace(IOFile file) : IEnumerable<IOFile>
{
    public IOFile File { get; } = file;

    public int Length => this.Count();

    public ProjectTrace Append(IOFile file) => new ChildTrace(file, this);

    public IEnumerator<IOFile> GetEnumerator() => Files().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected virtual IEnumerable<IOFile> Files() => [File];

    private sealed class ChildTrace(IOFile path, ProjectTrace parent) : ProjectTrace(path)
    {
        private readonly ProjectTrace Parent = parent;

        protected override IEnumerable<IOFile> Files() => [File, .. Parent.Files()];
    }
}
