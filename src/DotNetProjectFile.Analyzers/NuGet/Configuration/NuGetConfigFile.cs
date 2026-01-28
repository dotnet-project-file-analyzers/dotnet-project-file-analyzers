using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.NuGet.Configuration;

/// <summary>Represents the (root of a) NuGet configuration file.</summary>
public sealed class NuGetConfigFile : Node, ProjectFile
{
    private NuGetConfigFile(IOFile path, SourceText text, AdditionalText? additionalText)
       : this(path, text, XDocument.Parse(text.ToString(), LoadOptions), additionalText)
    {
    }

    private NuGetConfigFile(IOFile path, SourceText text, XDocument document, AdditionalText? additionalText)
        : base(document.Root, null, null)
    {
        Path = path;
        Text = text;
        AdditionalText = additionalText;
        WarningPragmas = WarningPragmas.New(this);
    }

    public AdditionalText? AdditionalText { get; }

    /// <inheritdoc />
    public IOFile Path { get; }

    /// <inheritdoc />
    public SourceText Text { get; }

    /// <inheritdoc />
    public WarningPragmas WarningPragmas { get; }

    public Nodes<PackageSources> PackageSources => new(Children);

    public Nodes<PackageSourceMapping> PackageSourceMappings => new(Children);

    public static NuGetConfigFile Load(IOFile file)
    {
        using var reader = file.TryOpenText();
        return new(
            path: file,
            text: SourceText.From(reader.ReadToEnd()),
            additionalText: null);
    }

    public static NuGetConfigFile Load(AdditionalText text) => new(
        path: IOFile.Parse(text.Path),
        text: text.GetText()!,
        additionalText: text);

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
