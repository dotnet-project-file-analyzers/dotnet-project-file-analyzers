using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Project : Node
{
    public Project(XElement element) : base(element) { }

    public Nodes<PropertyGroup> PropertyGroups => GetChildren<PropertyGroup>();

    public Nodes<ItemGroup> ItemGroups => GetChildren<ItemGroup>();

    public static Project Load(FileInfo file)
        => new(XElement.Load(file.OpenRead(), LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo));

    internal static Project Load(AdditionalText text)
        => new(XElement.Parse(text.GetText()?.ToString()));
}
