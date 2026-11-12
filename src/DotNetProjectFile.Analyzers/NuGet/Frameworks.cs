using Version = System.Version;

namespace DotNetProjectFile.NuGet;

/// <summary>Minimal target framework parsing and compatibility, as used for nuspec dependency groups.</summary>
/// <remarks>
/// ponytail: covers .NET Framework, .NET Standard, .NET Core and .NET 5+.
/// Other frameworks (Xamarin, UWP, portable profiles) are not recognized;
/// callers fall back to all dependency groups for those. Use NuGet.Frameworks
/// if that ever proves to be too limited.
/// </remarks>
public static class Frameworks
{
    private const string NetFramework = "netframework";
    private const string NetStandard = "netstandard";
    private const string NetCoreApp = "netcoreapp";

    /// <summary>A parsed target framework.</summary>
    public readonly record struct Framework(string Family, Version Version);

    /// <summary>Parses both short (net8.0, net472) and long (.NETFramework4.7.2) notations.</summary>
    [Pure]
    public static Framework? Parse(string? tfm)
    {
        if (tfm?.Trim().ToLowerInvariant() is not { Length: > 0 } s) return null;

        // Strip the platform: net8.0-windows => net8.0.
        if (s.IndexOf('-') is var dash and > 0) s = s[..dash];

        // Strip the leading period: .netcoreapp3.0 => netcoreapp3.0.
        s = s.TrimStart('.');

        return s switch
        {
            _ when s.StartsWith(NetFramework, StringComparison.OrdinalIgnoreCase) => Create(NetFramework, s[NetFramework.Length..]),
            _ when s.StartsWith(NetStandard, StringComparison.OrdinalIgnoreCase) => Create(NetStandard, s[NetStandard.Length..]),
            _ when s.StartsWith(NetCoreApp, StringComparison.OrdinalIgnoreCase) => Create(NetCoreApp, s[NetCoreApp.Length..]),
            _ when s.StartsWith("net") => Net(s[3..]),
            _ => null,
        };
    }

    /// <summary>
    /// Selects the nearest compatible framework from the <paramref name="candidates"/>,
    /// preferring the same family over .NET Standard, and the highest version.
    /// </summary>
    /// <returns>The index of the nearest candidate, or -1 if none is compatible.</returns>
    [Pure]
    public static int Nearest(Framework project, IReadOnlyList<Framework?> candidates)
    {
        var best = -1;
        var bestScore = (Rank: 0, Version: new Version());

        for (var i = 0; i < candidates.Count; i++)
        {
            if (candidates[i] is not { } candidate) continue;

            var rank = Rank(project, candidate);

            if (rank > 0 && (best < 0 || rank > bestScore.Rank || (rank == bestScore.Rank && candidate.Version > bestScore.Version)))
            {
                best = i;
                bestScore = (rank, candidate.Version);
            }
        }

        return best;
    }

    /// <returns>2 for the same family, 1 for a compatible .NET Standard, 0 if incompatible.</returns>
    private static int Rank(Framework project, Framework candidate) => candidate.Family switch
    {
        _ when candidate.Family == project.Family => candidate.Version <= project.Version ? 2 : 0,
        NetStandard when MaxNetStandard(project) is { } max && candidate.Version <= max => 1,
        _ => 0,
    };

    /// <summary>The highest .NET Standard version the framework implements.</summary>
    private static Version? MaxNetStandard(Framework project) => project.Family switch
    {
        NetCoreApp when project.Version.Major >= 3 => new(2, 1),
        NetCoreApp when project.Version.Major >= 2 => new(2, 0),
        NetCoreApp => new(1, 6),
        NetFramework when project.Version >= new Version(4, 6, 1) => new(2, 0),
        NetFramework when project.Version >= new Version(4, 6) => new(1, 3),
        NetFramework when project.Version >= new Version(4, 5) => new(1, 1),
        _ => null,
    };

    /// <summary>net472 is .NET Framework; net5.0 and up are .NET (Core).</summary>
    private static Framework? Net(string version)
    {
        if (version.IndexOf('.') >= 0)
        {
            return Create(Version.TryParse(version, out var v) && v.Major >= 5 ? NetCoreApp : NetFramework, version);
        }

        // Short notation: net472 => 4.7.2, net48 => 4.8.
        return version.Length is >= 2 and <= 3 && version.All(char.IsDigit)
            ? Create(NetFramework, string.Join(".", version.ToCharArray()))
            : null;
    }

    private static Framework? Create(string family, string version)
    {
        var v = version.TrimStart('v');

        // netstandard2 => 2.0
        if (v.Length > 0 && v.All(char.IsDigit)) v += ".0";

        return Version.TryParse(v, out var parsed) ? new(family, parsed) : null;
    }
}
