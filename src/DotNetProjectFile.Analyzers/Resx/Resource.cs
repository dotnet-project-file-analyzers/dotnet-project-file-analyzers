using Microsoft.CodeAnalysis.Text;
using System.Globalization;
using System.Xml;

namespace DotNetProjectFile.Resx;

[DebuggerDisplay("Culture = {Culture.Name}, Count = {Data.Count}")]
public sealed class Resource : Node, ProjectFile
{
    private readonly Dictionary<string, Data> lookup = [];

    public Resource(
        ResourceFileInfo path,
        XElement element,
        SourceText sourceText,
        bool isXml,
        ProjectFiles resources)
        : base(element, null)
    {
        Path = path;
        Text = sourceText;
        IsXml = isXml;
        ProjectFiles = resources;

        foreach (var data in Data.Where(d => d.Name is { Length: > 0 }))
        {
            lookup[data.Name!] = data;
        }
    }

    IOFile ProjectFile.Path => Path;

    public ResourceFileInfo Path { get; }

    public SourceText Text { get; }

    public WarningPragmas WarningPragmas { get; } = WarningPragmas.None;

    public CultureInfo Culture => Path.Culture;

    public Nodes<ResHeader> Headers => new(Children);

    public Nodes<Data> Data => new(Children);

    public IEnumerable<Resource> Parents
        => Culture.Ancestors()
        .Select(Path.Satellite)
        .Select(file => ProjectFiles.ResourceFile(file))
        .OfType<Resource>();

    private readonly ProjectFiles ProjectFiles;

    public bool IsXml { get; }

    public bool ForInvariantCulture => Culture.IsInvariant();

    public bool Contains(string? name) => name is { } && lookup.ContainsKey(name);

    public static Resource Load(AdditionalText text, ProjectFiles projectFiles)
    {
        var file = new ResourceFileInfo(text.Path);
        var sourceText = text.GetText()!;
        var isXml = TryElement(sourceText, out var element);
        return new(file, element, sourceText, isXml, projectFiles);
    }

    public static Resource Load(IOFile file, ProjectFiles projectFiles)
    {
        var info = new ResourceFileInfo(file);
        using var stream = file.OpenRead();
        var sourceText = SourceText.From(stream);
        var isXml = TryElement(sourceText, out var element);
        return new(info, element, sourceText, isXml, projectFiles);
    }

    private static bool TryElement(SourceText sourceText, out XElement element)
    {
        try
        {
            element = XElement.Parse(sourceText.ToString(), LoadOptions);
            return true;
        }
        catch (XmlException)
        {
            element = XElement.Parse(@"<root />", LoadOptions);
            return false;
        }
    }

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
