using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.CodeAnalysis;

public interface ProjectFile
{
    /// <summary>The path to the project file.</summary>
    IOFile Path { get; }

    /// <summary>The source text of the project file.</summary>
    SourceText Text { get; }

    /// <summary>The warning pragma's defined in the project file.</summary>
    WarningPragmas WarningPragmas { get; }
}
