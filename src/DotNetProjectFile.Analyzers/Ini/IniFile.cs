using Antlr4.Runtime;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Ini;

public sealed class IniFile : ProjectFile
{
	public required IniFileSyntax Sytnax { get; init; }

    /// <inheritdoc />
    public IOFile Path { get; init; }

    /// <inheritdoc />
    public required SourceText Text { get; init; }

    /// <inheritdoc />
    public WarningPragmas WarningPragmas => WarningPragmas.None;

    public static IniFile Load(IOFile path)
	{
        using var stream = path.OpenRead();
        var source = SourceText.From(stream);

		return new()
		{
			Sytnax = IniFileSyntax.Parse(source),
            Text = source,
		};
	}
}
