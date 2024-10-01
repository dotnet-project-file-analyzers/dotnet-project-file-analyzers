using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.IO;

public sealed class ProjectTextFile(IOFile path) : ProjectFile
{
    public IOFile Path { get; } = path;

    public SourceText Text => SourceText.From(Path.OpenRead());

    public WarningPragmas WarningPragmas { get; } = WarningPragmas.None;
}
