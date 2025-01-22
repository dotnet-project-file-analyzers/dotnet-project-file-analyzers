using DotNetProjectFile.Ini;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.EditorConfig;

public sealed class EditorConfigFile(OldIniFileSyntax syntax) : ProjectFile
{
    /// <summary>INI syntax.</summary>
    public OldIniFileSyntax Syntax { get; } = syntax;

    /// <inheritdoc />
    public IOFile Path => Syntax.SyntaxTree.Path;

    /// <inheritdoc />
    public SourceText Text => Syntax.SyntaxTree.SourceText;

    /// <inheritdoc />
    public WarningPragmas WarningPragmas => WarningPragmas.None;

    public bool IsRoot
        => Syntax.Sections.FirstOrDefault() is { } section
        && section.Header is null
        && section.Kvps
            .Where(kvp => kvp.Key.IsMatch("root"))
            .Select(kvp => kvp.Value.IsMatch("true"))
            .LastOrDefault();
}
