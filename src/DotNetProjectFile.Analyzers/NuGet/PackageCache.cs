#pragma warning disable RS1035 // Do not use APIs banned for analyzers
// Environment is required to run this logic
using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet.Packaging;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DotNetProjectFile.NuGet;

public static class PackageCache
{
    private static readonly ConcurrentDictionary<PackageVersionInfo, Package?> cache = new();
    private static readonly Lazy<IODirectory> cacheDir = new(() => IODirectory.Parse(GetPathInternal()));

    public static IODirectory GetDirectory()
        => cacheDir.Value;

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

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return @"%userprofile%\.nuget\packages";
        }
        else
        {
            return @"~/.nuget/packages";
        }
    }

    public static Package? GetPackage(string? name, string? version) => GetPackage(new(name!, version));

    public static Package? GetPackage(PackageVersionInfo info)
    {
        if (info is not { Name.Length: > 0 }) return null;

        // Directories have lower case names.
        var name = info.Name.ToLowerInvariant();
        var version = info.Version?.ToLowerInvariant();

        if (cache.TryGetValue(info, out var result))
        {
            return result;
        }

        lock (cache)
        {
            if (!cache.TryGetValue(info, out result))
            {
                result = GetPackageInternal(name, version);

                cache[info] = result;

                if (result is { } && version != result.Version)
                {
                    cache[new(name, result.Version)] = result;
                }
            }
        }

        return result;
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
            HasAnalyzerDll = HasDllFiles("analyzers"),
            HasRuntimeDll = HasDllFiles("lib") || HasDllFiles("runtimes"),
            HasDependencies = nuspec?.Metadata?.Depedencies?.SelectMany(d => d.Dependencies ?? [])?.Any() is true,
            IsDevelopmentDependency = nuspec?.Metadata?.DevelopmentDependency,
            LicenseExpression = licenseExpression,
            LicenseFile = licenseFile,
            LicenseUrl = nuspec?.Metadata?.LicenseUrl,
            License = license,
            NuSpec = nuspec,
        };

        bool HasDllFiles(string subDir)
            => versionDir.SubDirectory(subDir) is { Exists: true } dir
            && dir.Files("./**/*.dll").Any();
    }

    private static IODirectory? GetVersionDirectory(IODirectory packageDir, string? versionLabel)
    {
        // TODO: resolve version to a suitable fixed version when floating version is provided. Issue #320

        if (versionLabel is { })
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
            foundVersion ??= pairs
                .OrderByDescending(pair => pair.Version)
                .FirstOrNone(pair => pair.Version < version)?
                .Directory;
        }

        return foundVersion;
    }

    private readonly record struct VersionLocation(IODirectory Directory, System.Version? Version);

    private static System.Version? TryParseVersion(string version)
    {
        try
        {
            return new(version);
        }
        catch
        {
            return null;
        }
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
        if (info is { Version.Length: > 0 })
        {
            yield return directory.File($"{info.Name.ToLowerInvariant()}.{info.Version.ToLowerInvariant()}.nuspec".ToLowerInvariant());
        }

        yield return directory.File($"{info.Name.ToLowerInvariant()}.nuspec".ToLowerInvariant());

        foreach (var file in directory.Files("*.nuspec") ?? [])
        {
            yield return file;
        }
    }
}
