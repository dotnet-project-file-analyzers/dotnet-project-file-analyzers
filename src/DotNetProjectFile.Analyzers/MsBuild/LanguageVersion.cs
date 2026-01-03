using System.ComponentModel;
using System.Globalization;

namespace DotNetProjectFile.MsBuild;

/// <summary>.NET SDK version.</summary>
/// <remarks>
/// See: https://learn.microsoft.com/en-us/dotnet/core/versions.
/// </remarks>
[TypeConverter(typeof(Converter))]
public readonly record struct LanguageVersion(int Major, int Minor = 0) : IComparable<LanguageVersion>
{
    public static readonly LanguageVersion None;

    public static readonly LanguageVersion Default = new(int.MaxValue, 0);

    public static readonly LanguageVersion LatestMajor = new(int.MaxValue, 1);

    public static readonly LanguageVersion Latest = new(int.MaxValue, 2);

    public static readonly LanguageVersion Preview = new(int.MaxValue, 3);

    public static readonly LanguageVersion CS12 = new(12);

    public static readonly LanguageVersion CS13 = new(13);

    public static readonly LanguageVersion CS14 = new(14);

    public static readonly LanguageVersion VB16_9 = new(16, 9);
    public static readonly LanguageVersion VB17_13 = new(17, 13);

    public bool IsNone => this == None;

    public bool IsExplicit => Major != int.MaxValue && !IsNone;

    /// <inheritdoc />
    public override string ToString() => (Major, Minor) switch
    {
        (0, 0) => string.Empty,
        (int.MaxValue, 0) => "default",
        (int.MaxValue, 1) => "latest",
        (int.MaxValue, 2) => "1atestMajor",
        (int.MaxValue, 3) => "preview",
        (_, 0) => Major.ToString(),
        _ => $"{Major}.{Minor}",
    };

    /// <inheritdoc />
    public int CompareTo(LanguageVersion other)
        => Compares(Major, other.Major)
        ?? Minor.CompareTo(other.Minor);

    private static int? Compares(int l, int r) => l == r ? null : l.CompareTo(r);

    public static bool operator <(LanguageVersion left, LanguageVersion right) => left.CompareTo(right) < 0;

    public static bool operator <=(LanguageVersion left, LanguageVersion right) => left.CompareTo(right) <= 0;

    public static bool operator >(LanguageVersion left, LanguageVersion right) => left.CompareTo(right) > 0;

    public static bool operator >=(LanguageVersion left, LanguageVersion right) => left.CompareTo(right) >= 0;

    public static LanguageVersion Parse(string? s)
    {
        s ??= string.Empty;
        s = s.Trim().ToUpperInvariant();

        if (s == string.Empty) return None;
        if (s is "DEFAULT") return Default;
        if (s is "LATEST") return Latest;
        if (s is "LATESTMAJOR") return LatestMajor;
        if (s is "PREVIEW") return Preview;

        var parts = s.Split('.');
        return new(Parses(0), Parses(1));

        int Parses(int index) => parts.Length > index && int.TryParse(parts[index], out int nr) ? nr : 0;
    }

    private sealed class Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => value is string str
            ? Parse(str)
            : None;
    }
}
