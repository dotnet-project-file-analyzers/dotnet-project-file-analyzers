using Microsoft.CodeAnalysis.Text;
using System.Globalization;
using System.Xml;

namespace DotNetProjectFile.Resx;

[DebuggerDisplay("Culture = {Culture.Name}, Count = {Data.Count}")]
public sealed class Resource : Node
{
    private readonly Dictionary<string, Data> lookup = [];

    public Resource(
        ResourceFileInfo path,
        XElement element,
        SourceText sourceText,
        bool isXml,
        Resources resources)
        : base(element, null)
    {
        Path = path;
        Text = sourceText;
        IsXml = isXml;
        Resources = resources;

        foreach (var data in Data.Where(d => d.Name is { Length: > 0 }))
        {
            lookup[data.Name!] = data;
        }
    }

    public ResourceFileInfo Path { get; }

    public SourceText Text { get; }

    public CultureInfo Culture => Path.Culture;

    public Nodes<ResHeader> Headers => new(Children);

    public Nodes<Data> Data => new(Children);

    public IReadOnlyCollection<Resource> Parents => parents ??= Resources.Parents(this);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IReadOnlyCollection<Resource>? parents;

    private readonly Resources Resources;

    public bool IsXml { get; }

    public bool ForInvariantCulture => Culture.IsInvariant();

    public bool Contains(string? name) => name is { } && lookup.ContainsKey(name);

    public static Resource Load(AdditionalText text, Resources resources)
    {
        var file = new ResourceFileInfo(text.Path);
        var sourceText = text.GetText()!;
        var isXml = TryElement(sourceText, out var element);
        return new(file, element, sourceText, isXml, resources);
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
