using Buildalyzer;
using Buildalyzer.IO;
using System.Diagnostics;
using System.IO;

namespace Specs.TestTools;

public sealed class BuildalyzerContext : IDisposable
{
    [Pure]
    public static BuildalyzerContext ForProject(string path, AnalyzerManagerOptions? options = null) => new(GetProjectPath(path), options);

    private static FileInfo GetProjectPath(string file)
    {
        var location = new FileInfo(typeof(BuildalyzerContext).Assembly.Location).Directory!;
        return new FileInfo(Path.Combine(
            location.FullName,
            "..",
            "..",
            "..",
            "..",
            "..",
            "projects",
            file));
    }

    private BuildalyzerContext(FileInfo location, AnalyzerManagerOptions? options)
    {
        Location = location;
        Manager = new AnalyzerManager(options ?? new AnalyzerManagerOptions { LogWriter = new StringWriter() });
        Analyzer = Manager.GetProject(IOPath.Parse(Location.FullName))!;
        DebugMode(ref InDebugMode);
        DeleteSubDirectory("artifacts");
        DeleteSubDirectory("bin");
        DeleteSubDirectory("obj");
    }

    public IProjectAnalyzer Analyzer { get; }

    public AnalyzerManager Manager { get; }

    public TextWriter Log { get => IsDisposed ? throw new ObjectDisposedException(GetType().FullName) : field; } = new StringWriter();

    public FileInfo Location { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!IsDisposed)
        {
            if (InDebugMode)
            {
                Console.WriteLine(Log.ToString());
            }
            Log.Dispose();

            IsDisposed = true;
        }
        GC.SuppressFinalize(this);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool IsDisposed;

    /// <summary>Ensures that the analysis is done ignoring previous results.</summary>
    private void DeleteSubDirectory(string path)
    {
        var directory = new DirectoryInfo(Path.Combine(Location.Directory!.FullName, path));

        if (directory.Exists)
        {
            try
            {
                directory.Delete(true);
                Log.WriteLine($"Deleted all files at {directory}");
            }
            catch (Exception x)
            {
                Log.WriteLine(x);
            }
        }
    }

    /// <summary>Sets <paramref name="inDebugMode"/> to true when run in DEBUG mode.</summary>
    [Conditional("DEBUG")]
    private static void DebugMode(ref bool inDebugMode) => inDebugMode = true;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly bool InDebugMode;
}
