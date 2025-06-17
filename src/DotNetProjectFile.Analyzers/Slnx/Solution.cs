using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Slnx;

public sealed class Solution : Node, ProjectFile
{
    private Solution(IOFile path, SourceText text, ProjectFiles projectFiles, AdditionalText? additionalText)
       : this(path, text, XDocument.Parse(text.ToString(), LoadOptions), projectFiles, additionalText)
    {
    }

    private Solution(IOFile path, SourceText text, XDocument document, ProjectFiles projectFiles, AdditionalText? additionalText)
        : base(document.Root, null, null)
    {
        Path = path;
        Text = text;
        ProjectFiles = projectFiles;
        AdditionalText = additionalText;
        WarningPragmas = WarningPragmas.New(this);
    }

    public AdditionalText? AdditionalText { get; }

    public IOFile Path { get; }

    public SourceText Text { get; }

    public WarningPragmas WarningPragmas { get; }

    internal ProjectFiles ProjectFiles { get; }

    public static Solution Load(IOFile file, ProjectFiles projects)
    {
        using var reader = file.TryOpenText();
        return new(
            path: file,
            text: SourceText.From(reader.ReadToEnd()),
            projectFiles: projects,
            additionalText: null);
    }

    public static Solution Load(AdditionalText text, ProjectFiles projects)
        => new(
            path: IOFile.Parse(text.Path),
            text: text.GetText()!,
            projectFiles: projects,
            additionalText: text);

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
