using Microsoft.CodeAnalysis.Text;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace DotNetProjectFile.Resx;

public sealed class Resource : Node
{
    public Resource(
        ResourceFileInfo path,
        XElement element,
        SourceText sourceText,
        bool isXml,
        Resources resources)
        : base(element, null)
    {
        Path = path;
        SourceText = sourceText;
        IsXml = isXml;
        Resources = resources;
        Headers = Children<ResHeader>();
        Data = Children<Data>();
    }

    public ResourceFileInfo Path { get; }

    public SourceText SourceText { get; }

    public CultureInfo Culture => Path.Culture;

    public Nodes<ResHeader> Headers { get; }

    public Nodes<Data> Data { get; }

    public IReadOnlyCollection<Resource> Parents => parents ??= Resources.Parents(this);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IReadOnlyCollection<Resource>? parents;

    private readonly Resources Resources;

    public bool IsXml { get; }

    public bool ForInvariantCulture => Culture.IsInvariant();

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
