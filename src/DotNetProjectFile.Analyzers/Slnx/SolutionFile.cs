using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Slnx;

public sealed class SolutionFile : Node, ProjectFile
{
    private SolutionFile(IOFile path, SourceText text, ProjectFiles projectFiles, AdditionalText? additionalText)
       : this(path, text, XDocument.Parse(text.ToString(), LoadOptions), projectFiles, additionalText)
    {
    }

    private SolutionFile(IOFile path, SourceText text, XDocument document, ProjectFiles projectFiles, AdditionalText? additionalText)
        : base(document.Root, null, null)
    {
        Path = path;
        Text = text;
        ProjectFiles = projectFiles;
        AdditionalText = additionalText;
        WarningPragmas = WarningPragmas.New(this);
    }

    public override string LocalName => "Solution";

    public AdditionalText? AdditionalText { get; }

    public IOFile Path { get; }

    public SourceText Text { get; }

    public WarningPragmas WarningPragmas { get; }

    public IEnumerable<SlnxProject> Projects => Children.OfType<SlnxProject>();

    internal ProjectFiles ProjectFiles { get; }

    public static SolutionFile Load(IOFile file, ProjectFiles projects) => new(
        path: file,
        text: file.SourceText(),
        projectFiles: projects,
        additionalText: null);

    public static SolutionFile Load(AdditionalText text, ProjectFiles projects)
        => new(
            path: text.Location,
            text: text.GetText()!,
            projectFiles: projects,
            additionalText: text);

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
