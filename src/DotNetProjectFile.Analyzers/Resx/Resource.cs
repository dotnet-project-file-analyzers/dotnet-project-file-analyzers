using Microsoft.CodeAnalysis.Text;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DotNetProjectFile.Resx;

public sealed class Resource : Node
{
    public Resource(FileInfo path, XElement element, SourceText sourceText, CultureInfo culture, bool isXml)
        : base(element, null)
    {
        Path = path;
        SourceText = sourceText;
        Culture = culture;
        IsXml = isXml;
        Headers = Children<ResHeader>();
        Data = Children<Data>();
    }

    public FileInfo Path { get; }

    public SourceText SourceText { get; }

    public CultureInfo Culture { get; }

    public Nodes<ResHeader> Headers { get; }

    public Nodes<Data> Data { get; }

    public bool IsXml { get; }

    public static Resource Load(AdditionalText text)
    {
        var file = new FileInfo(text.Path);
        var sourceText = text.GetText()!;
        var isXml = TryElement(sourceText, out var element);
        var culture = TryCulture(file);
        return new(file, element, sourceText, culture, isXml);
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

    private static CultureInfo TryCulture(FileInfo file)
    {
        var parts = file.Name.Split('.');

        if (parts.Length > 2)
        {
            try
            {
                return CultureInfo.GetCultureInfo(parts[^2]);
            }
            catch (CultureNotFoundException)
            {
                // not a culture.
            }
        }
        return CultureInfo.InvariantCulture;
    }

    private static readonly LoadOptions LoadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;
}
