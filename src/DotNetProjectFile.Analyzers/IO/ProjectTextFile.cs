using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.IO;

/// <summary>Represents a (generic) project text file.</summary>
public sealed class ProjectTextFile(IOFile path) : ProjectFile
{
    /// <inheritdoc />
    public IOFile Path { get; } = path;

    /// <inheritdoc />
    public SourceText Text => SourceText.From(Path.OpenRead());

    /// <inheritdoc />
    public WarningPragmas WarningPragmas { get; } = WarningPragmas.None;
}
