namespace DotNetProjectFile.Resx;

public sealed class Data : Node
{
    public Data(XElement element, Resource? resource)
        : base(element, resource)
    {
        Name = element.Attribute("name")?.Value;
        Value = Children.OfType<Value>().FirstOrDefault();
    }

    public string? Name { get; }

    public Value? Value { get; }
}
