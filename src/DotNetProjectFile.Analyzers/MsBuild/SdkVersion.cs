namespace DotNetProjectFile.MsBuild;

/// <summary>.NET SDK version.</summary>
/// <remarks>
/// See: https://learn.microsoft.com/en-us/dotnet/core/versions.
/// </remarks>
public readonly record struct SdkVersion(int Major, int Minor, int Patch) : IComparable<SdkVersion>
{
    public static readonly SdkVersion None;
    
    /// <summary>The .NET 7.0 SDK.</summary>
    public static readonly SdkVersion NET6 = new(6, 0, 0);

    /// <summary>The .NET 8.0 SDK.</summary>
    public static readonly SdkVersion NET7 = new(7, 0, 0);

    /// <summary>The .NET 8.0 SDK.</summary>
    public static readonly SdkVersion NET8 = new(8, 0, 0);

    /// <summary>The .NET 9.0 SDK.</summary>
    public static readonly SdkVersion NET9 = new(9, 0, 0);

    /// <summary>The .NET 10.0 SDK.</summary>
    public static readonly SdkVersion NET10 = new(10, 0, 0);

    public bool IsNone => this == None;

    /// <inheritdoc />
    public override string ToString() => IsNone ? string.Empty : $"{Major}.{Minor}.{Patch}";

    /// <inheritdoc />
    public int CompareTo(SdkVersion other)
        => Compares(Major, other.Major)
        ?? Compares(Minor, other.Minor)
        ?? Patch.CompareTo(other.Patch);

    private static int? Compares(int l, int r) => l == r ? null : l.CompareTo(r);

    public static bool operator <(SdkVersion left, SdkVersion right) => left.CompareTo(right) < 0;

    public static bool operator <=(SdkVersion left, SdkVersion right) => left.CompareTo(right) <= 0;

    public static bool operator >(SdkVersion left, SdkVersion right) => left.CompareTo(right) > 0;

    public static bool operator >=(SdkVersion left, SdkVersion right) => left.CompareTo(right) >= 0;

    public static SdkVersion Parse(string s)
    {
        var parts = s.Split('.', '-');
        return new(Parses(0), Parses(1), Parses(2));

        int Parses(int index) => parts.Length > index && int.TryParse(parts[index], out int nr) ? nr : 0;
    }
}
