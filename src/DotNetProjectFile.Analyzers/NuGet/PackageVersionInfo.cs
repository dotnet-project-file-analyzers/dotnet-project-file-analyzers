namespace DotNetProjectFile.NuGet;

/// <summary>Represents the package name and (optional) version.</summary>
public readonly struct PackageVersionInfo : IEquatable<PackageVersionInfo>
{
    public PackageVersionInfo(string name, string? version = null)
    {
        Name = name;
        Version = version;
    }

    /// <summary>The name of the package.</summary>
    public readonly string Name;

    /// <summary>The version of the package.</summary>
    public readonly string? Version;

    /// <summary>Checks if the include or update matches the name of this package.</summary>
    [Pure]
    public bool IsMatch(PackageReferenceBase reference) => reference.IncludeOrUpdate.IsMatch(Name);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
    {
        var hash = Name?.ToUpperInvariant().GetHashCode() ?? 0;
        hash *= 17;
        hash ^= Version?.ToUpperInvariant().GetHashCode() ?? 0;
        return hash;
    }

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is PackageVersionInfo other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(PackageVersionInfo other)
        => Name.IsMatch(other.Name)
        && Version.IsMatch(other.Version);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Version is null ? Name : $"{Name} ({Version})";

    public static bool operator ==(PackageVersionInfo left, PackageVersionInfo right) => left.Equals(right);

    public static bool operator !=(PackageVersionInfo left, PackageVersionInfo right) => !(left == right);
}
