using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.EditorConfig;

public sealed class EditorConfigFile(IniFile syntax) : ProjectFile
{
    /// <summary>INI syntax.</summary>
    public IniFile Syntax { get; } = syntax;

    /// <inheritdoc />
    public IOFile Path => Syntax.SourceTree.Path;

    /// <inheritdoc />
    public SourceText Text => Syntax.SourceTree.SourceText;

    /// <inheritdoc />
    public WarningPragmas WarningPragmas => WarningPragmas.None;

    public bool IsRoot => Syntax.IsRoot;
}
