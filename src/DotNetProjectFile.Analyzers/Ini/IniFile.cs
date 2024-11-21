using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Ini;

public sealed class IniFile(IniFileSyntax syntax) : ProjectFile
{
    /// <summary>INI syntax.</summary>
    public IniFileSyntax Syntax { get; } = syntax;

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
