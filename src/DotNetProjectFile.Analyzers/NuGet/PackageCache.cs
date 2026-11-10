#pragma warning disable RS1035 // Do not use APIs banned for analyzers
// Environment is required to run this logic
using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet.Packaging;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;

namespace DotNetProjectFile.NuGet;

public static class PackageCache
{
    private static readonly ConcurrentDictionary<PackageVersionInfo, Lazy<Package?>> cache = new();
    private static readonly Lazy<IODirectory> CacheDir = new(() => IODirectory.Parse(GetPathInternal()));

    public static IODirectory GetDirectory()
        => CacheDir.Value;

    private static string GetPathInternal()
    {
        var raw = GetRawPath();
        var envExpanded = Environment.ExpandEnvironmentVariables(raw);
        var tildeExpanded = envExpanded.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).TrimEnd('/'));
        var full = Path.GetFullPath(tildeExpanded);
        return full;
    }

    private static string GetRawPath()
    {
        if (Environment.GetEnvironmentVariable("NUGET_PACKAGES") is { Length: > 0 } fromEnv)
        {
            return fromEnv;
        }

        // TODO: handle `globalPackagesFolder` given by `nuget.config`. Issue #319
        // TODO: handle `repositoryPath` given by `packages.config`. Issue #318
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? @"%userprofile%\.nuget\packages"
            : @"~/.nuget/packages";
    }

    public static Package? GetPackage(string? name, string? version)
        => name is { } n
        ? GetPackage(new(n, version))
        : null;

    /// <remarks>
    /// Only requests for the same package (version) wait for each other.
    /// </remarks>
    public static Package? GetPackage(PackageVersionInfo info)
        => info is { Name.Length: > 0 }
        ? cache.GetOrAdd(info, i => new(() => Resolve(i))).Value
        : null;

    private static Package? Resolve(PackageVersionInfo info)
    {
        // Directories have lower case names.
        var name = info.Name.ToLowerInvariant();

        if (GetPackageInternal(name, info.Version?.ToLowerInvariant()) is not { } package)
        {
            return null;
        }

        var resolved = new PackageVersionInfo(name, package.Version);

        // Different versions (ranges) resolving to the same version share one instance.
        return resolved == info
            ? package
            : cache.GetOrAdd(resolved, new Lazy<Package?>(() => package)).Value;
    }

    private static Package? GetPackageInternal(string name, string? version)
    {
        var cacheDir = GetDirectory();
        var packageDir = cacheDir.SubDirectory(name);

        if (!packageDir.Exists)
        {
            return null;
        }

        if (GetVersionDirectory(packageDir, version) is not { } versionDir)
        {
            return null;
        }

        var nuspec = TryLoadNuSpecFile(versionDir, new(name, versionDir.Name));

        var licenseNode = nuspec?.Metadata?.License;
        var licenseUrl = nuspec?.Metadata?.LicenseUrl;

        var licenseExpression = licenseNode is { Type: "expression" }
            ? licenseNode.Value
            : null;
        var licenseFile = licenseNode is { Type: "file" }
            ? licenseNode.Value
            : null;

        var license = Licenses.FromExpression(licenseExpression);

        if (license == Licenses.Unknown)
        {
            license = Licenses.FromUrl(licenseUrl);
        }

        if (license == Licenses.Unknown && licenseFile is { Length: > 0 })
        {
            license = Licenses.FromFile(versionDir.File(licenseFile));
        }

        return new()
        {
            Name = name,
            Version = versionDir.Name,
            Directory = versionDir,
            HasDependencies = nuspec?.Metadata?.Dependencies?.All.Any() is true,
            IsDevelopmentDependency = nuspec?.Metadata?.DevelopmentDependency,
            LicenseExpression = licenseExpression,
            LicenseFile = licenseFile,
            LicenseUrl = nuspec?.Metadata?.LicenseUrl,
            License = license,
            NuSpec = nuspec,
        };
    }

    private static IODirectory? GetVersionDirectory(IODirectory packageDir, string? versionLabel)
    {
        // TODO: resolve version to a suitable fixed version when floating version is provided. Issue #320
        (versionLabel, var exclusive) = LowerBound(versionLabel);

        // An exclusive lower bound, such as (1.0, 2.0), excludes the version itself.
        if (versionLabel is { } && !exclusive)
        {
            var exactVersionDir = packageDir.SubDirectory(versionLabel);
            if (exactVersionDir.Exists)
            {
                return exactVersionDir;
            }
        }

        var version = versionLabel is null ? null : TryParseVersion(versionLabel);

        IODirectory? foundVersion = null;
        if (version is null)
        {
            // Default to highest found package version if the input version was gibberish.
            foundVersion = packageDir.SubDirectories()
                .OrderByDescending(d => TryParseVersion(d.Name))
                .FirstOrNone();
        }
        else
        {
            var pairs = packageDir.SubDirectories()
                .Select(d => new VersionLocation(d, TryParseVersion(d.Name)))
                .Where(vl => vl.Version is { })
                .ToArray();

            // Pick nearest version that is higher than the current version if available.
            foundVersion = pairs
                .OrderBy(vl => vl.Version)
                .FirstOrNone(vl => vl.Version > version)?
                .Directory;

            // Pick nearest version that is lower than the current version if available.
            // For an exclusive lower bound, the bound itself is the nearest lower version.
            foundVersion ??= pairs
                .OrderByDescending(pair => pair.Version)
                .FirstOrNone(pair => pair.Version < version || (exclusive && pair.Version == version))?
                .Directory;
        }

        return foundVersion;
    }

    private readonly record struct VersionLocation(IODirectory Directory, System.Version? Version);

    private static System.Version? TryParseVersion(string version)
        => System.Version.TryParse(version, out var parsed) ? parsed : null;

    /// <summary>Gets the lower bound of a NuGet version range, such as [1.0, 2.0) or (1.0, 2.0).</summary>
    /// <remarks>
    /// NuGet resolves the lowest applicable version, which is the lower bound
    /// (when restored, and inclusive). Plain versions (1.0) are an inclusive
    /// lower bound already. No lower bound, such as (, 2.0], gives null.
    /// </remarks>
    [Pure]
    public static (string? Version, bool Exclusive) LowerBound(string? versionLabel)
    {
        if (versionLabel?.Trim() is not { Length: > 0 } label) return (null, false);

        if (label[0] is not '[' and not '(') return (label, false);

        var comma = label.IndexOf(',');
        var end = comma >= 0 ? comma : label.Length - 1;
        var lower = label.Substring(1, Math.Max(0, end - 1)).Trim();

        return lower is { Length: > 0 }
            ? (lower, label[0] == '(')
            : (null, false);
    }

    private static NuSpecFile? TryLoadNuSpecFile(IODirectory directory, PackageVersionInfo info)
    {
        if (NuspecFiles(directory, info).FirstOrNone(f => f.Exists) is { } file)
        {
            try
            {
                using var stream = file.OpenRead();
                return NuSpecFile.Load(stream);
            }
            catch
            {
                // We're not sure that loading will succeed.
            }
        }
        return null;
    }

    private static IEnumerable<IOFile> NuspecFiles(IODirectory directory, PackageVersionInfo info)
    {
        // The global packages folder always contains {id}.nuspec (lower case).
        yield return directory.File($"{info.Name.ToLowerInvariant()}.nuspec");

        if (info is { Version.Length: > 0 })
        {
            yield return directory.File($"{info.Name.ToLowerInvariant()}.{info.Version.ToLowerInvariant()}.nuspec");
        }

        foreach (var file in directory.Files("*.nuspec") ?? [])
        {
            yield return file;
        }
    }
}
