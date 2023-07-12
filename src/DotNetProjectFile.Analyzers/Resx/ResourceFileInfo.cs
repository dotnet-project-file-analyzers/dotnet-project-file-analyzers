using System.Globalization;
using System.IO;

namespace DotNetProjectFile.Resx;

public sealed class ResourceFileInfo
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly FileInfo Info;

    public ResourceFileInfo(string fileName) : this(new FileInfo(fileName)) { }

    public ResourceFileInfo(FileInfo info)
    {
        Info = info;
        Culture = TryCulture(Name);
    }

    public string Name => Info.Name;

    public string Extension => Info.Extension;

    public CultureInfo Culture { get; }

    public ResourceFileInfo Satellite(CultureInfo culture)
    {
        var name = Path.GetFileNameWithoutExtension(Name);
        name = name.Substring(0, name.Length - Culture.Name.Length - 1);
        name = Path.Combine(Info.Directory.FullName, name);

        return culture.IsInvariant()
            ? new($"{name}{Extension}")
            : new($"{name}.{culture}{Extension}");
    }

    public override string ToString() => Info.ToString();

    public static implicit operator FileInfo(ResourceFileInfo info) => info.Info;

    private static CultureInfo TryCulture(string name)
    {
        var parts = name.Split('.');

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
}
