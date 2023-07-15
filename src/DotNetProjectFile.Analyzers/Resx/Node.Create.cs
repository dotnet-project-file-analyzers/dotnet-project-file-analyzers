namespace DotNetProjectFile.Resx;

public partial class Node
{
    internal Node? Create(XElement element)
        => element.Name.LocalName switch
        {
            null => null,
            "resheader" /*.*/ => new ResHeader(element, Resource),
            "data" /*......*/ => new Data(element, Resource),
            "value" /*.....*/ => new Value(element, Resource),
            _ => new Unknown(element, Resource),
        };
}
