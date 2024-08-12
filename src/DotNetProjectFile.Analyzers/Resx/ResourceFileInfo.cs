using System.Globalization;

namespace DotNetProjectFile.Resx;

public sealed class ResourceFileInfo
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IOFile Info;

    public ResourceFileInfo(string fileName) : this(IOFile.Parse(fileName)) { }

    public ResourceFileInfo(IOFile info)
    {
        Info = info;
        Culture = TryCulture(Name);
    }

    public string Name => Info.NameWithoutExtension;

    public string Extension => Info.Extension;

    public CultureInfo Culture { get; }

    public ResourceFileInfo Satellite(CultureInfo culture)
    {
        var name = Info.Name[..(Name.Length - Culture.Name.Length - 1)];

        var path = culture.IsInvariant()
            ? Info.Directory.File($"{name}{Extension}")
            : Info.Directory.File($"{name}.{culture}{Extension}");

        return new(path);
    }

    public override string ToString() => Info.ToString();

    public static implicit operator IOFile(ResourceFileInfo info) => info.Info;

    private static CultureInfo TryCulture(string name)
    {
        var parts = name.Split('.');

        if (parts.Length > 1)
        {
            try
            {
                return CultureInfo.GetCultureInfo(parts[^1]);
            }
            catch (CultureNotFoundException)
            {
                // not a culture.
            }
        }
        return CultureInfo.InvariantCulture;
    }
}
