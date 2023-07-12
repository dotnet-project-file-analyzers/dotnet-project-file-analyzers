using Microsoft.CodeAnalysis.Text;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DotNetProjectFile.Resx;

public sealed class Resource : Node
{
    public Resource(FileInfo path, XElement element, SourceText sourceText, CultureInfo culture, XmlException? exception)
        : base(element, null)
    {
        Path = path;
        SourceText = sourceText;
        Culture = culture;
        Exception = exception;
        Data = Children<Data>();
    }

    public FileInfo Path { get; }

    public SourceText SourceText { get; }

    public CultureInfo Culture { get; }

    public Nodes<Data> Data { get; }

    public XmlException? Exception { get; }

    public static Resource Load(AdditionalText text)
    {
        var file = new FileInfo(text.Path);
        var sourceText = text.GetText()!;
        var element = TryElement(sourceText, out var exception);
        var culture = TryCulture(file);
        return new(file, element, sourceText, culture, exception);
    }

    private static XElement TryElement(SourceText sourceText, out XmlException? exception)
    {
        exception = null;
        try
        {
            return XElement.Parse(sourceText.ToString(), LoadOptions);
        }
        catch (XmlException x)
        {
            exception = x;
            return XElement.Parse(@"<root />", LoadOptions);
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
