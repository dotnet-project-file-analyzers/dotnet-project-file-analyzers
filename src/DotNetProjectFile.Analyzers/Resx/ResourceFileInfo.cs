using System.Collections.Frozen;
using System.Globalization;

namespace DotNetProjectFile.Resx;

/// <summary>Represents a RESX file.</summary>
public sealed class ResourceFileInfo(IOFile info)
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IOFile Info = info;

    /// <inheritdoc cref="IOFile.NameWithoutExtension" />
    public string Name => Info.NameWithoutExtension;

    /// <inheritdoc cref="IOFile.Extension" />
    public string Extension => Info.Extension;

    /// <summary>Gets the linked culture fo a file.</summary>
    public CultureInfo Culture => field ??= TryCulture(Info.NameWithoutExtension);

    /// <summary>Gets a satallite file.</summary>
    public ResourceFileInfo Satellite(CultureInfo culture)
    {
        var name = Info.Name[..(Name.Length - Culture.Name.Length - 1)];

        var path = culture.IsInvariant
            ? Info.Directory.File($"{name}{Extension}")
            : Info.Directory.File($"{name}.{culture}{Extension}");

        return new(path);
    }

    /// <inheritdoc />
    public override string ToString() => Info.ToString();

    /// <summary>Implicitly casts to a <see cref="IOFile" />.</summary>
    public static implicit operator IOFile(ResourceFileInfo info) => info.Info;

    private static CultureInfo TryCulture(string name)
        => name.Split('.') is {Length: > 1 } parts && Cultures.TryGetValue(parts[^1], out var culture)
        ? culture
        : CultureInfo.InvariantCulture;

    private static FrozenDictionary<string, CultureInfo> Cultures
        => field ??= CultureInfo
        .GetCultures(CultureTypes.AllCultures)
        .ToFrozenDictionary(c => c.Name, c => c, StringComparer.OrdinalIgnoreCase);
}
