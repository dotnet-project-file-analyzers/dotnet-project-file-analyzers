using DotNetProjectFile.MsBuild;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNetProjectFile.NuGet;

public static class PackageCache
{
    private static readonly ConcurrentDictionary<(string Name, string? Version), CachedPackage?> cache = new();
    private static readonly Lazy<string> path = new(GetPathInternal);

    public static string GetPath()
        => path.Value;

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

        // TODO: handle `globalPackagesFolder` given by `nuget.config`
        // TODO: handle `repositoryPath` given by `packages.config`

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return @"%userprofile%\.nuget\packages";
        }
        else
        {
            return @"~/.nuget/packages";
        }
    }

    public static CachedPackage? GetPackage(string? name, string? version)
    {
        if (name is not { Length: > 0 })
        {
            return null;
        }

        // Directories have lower case names.
        name = name.ToLowerInvariant();
        version = version?.ToLowerInvariant();

        var key = (name, version);
        if (cache.TryGetValue(key, out var result))
        {
            return result;
        }

        lock (cache)
        {
            if (!cache.TryGetValue(key, out result))
            {
                result = GetPackageInternal(name, version);

                cache[key] = result;
                
                if (result is { } && version != result.Version)
                {
                    cache[(name, result.Version)] = result;
                }
            }
        }

        return result;
    }

    private static CachedPackage? GetPackageInternal(string name, string? version)
    {
        var cacheDir = GetPath();
        var packageDir = Path.Combine(cacheDir, name);

        if (!Directory.Exists(packageDir))
        {
            return null;
        }

        var (dir, foundVersion) = GetVersionDirectory(packageDir, version);

        if (dir is null || foundVersion is null)
        {
            return null;
        }

        return new()
        {
            Name = name,
            Version = foundVersion,
            HasAnalyzerDll = HasDllFiles("analyzers"),
            HasRuntimeDll = HasDllFiles("lib") || HasDllFiles("runtimes"),
            License = GetLicense(dir, name),
        };

        bool HasDllFiles(string subDir)
        {
            var path = Path.Combine(dir, subDir);
            if (!Directory.Exists(path))
            {
                return false;
            }

            return Directory.GetFiles(Path.Combine(dir, subDir), "*.dll", SearchOption.AllDirectories).Length > 0;
        }
    }

    private static LicenseExpression GetLicense(string path, string packageName)
    {
        var nuspecFile = Path.Combine(path, $"{packageName}.nuspec");

        if (!File.Exists(nuspecFile))
        {
            return Licenses.Unknown;
        }

        try
        {
            var doc = XDocument.Load(nuspecFile);

            var metadata = doc.Root.Elements().FirstOrDefault(n => n.Name?.LocalName == "metadata");
            var z = doc.Root.Element("metadata");
            var licenseExpressionNode = metadata?.Element("license");
            var licenseExpressionNode2 = metadata?.Elements("license").ToArray();
            var licenseExpressionNode3 = metadata?.Elements().FirstOrDefault(n => n.Name?.LocalName == "license");

            throw new Exception();
        }
        catch
        {
            return Licenses.Unknown;
        }
    }

    private static (string? Directory, string? Version) GetVersionDirectory(string packageDir, string? version)
    {
        // TODO: resolve version to a suitable fixed version when floating version is provided.

        if (version is { })
        {
            var exactVersionDir = Path.Combine(packageDir, version);
            if (Directory.Exists(exactVersionDir))
            {
                return (exactVersionDir, version);
            }
        }

        var vVersion = version is null ? null : TryParseVersion(version);

        string? foundVersion;
        if (vVersion is null)
        {
            // Default to highest found package version if the input version was gibberish.
            foundVersion = Directory.GetFiles(packageDir).OrderBy(TryParseVersion).LastOrDefault();
        }
        else
        {
            var pairs = Directory.GetFiles(packageDir)
                .Select(str => (str, TryParseVersion(str)))
                .ToArray();

            // Pick nearest version that is higher than the current version if available.
            foundVersion = pairs
                .Where(pair => pair.Item2 > vVersion)
                .OrderBy(pair => pair.Item2)
                .FirstOrDefault().str;

            if (foundVersion is null)
            {
                // Pick nearest version that is lower than the current version if available.
                foundVersion = pairs
                    .Where(pair => pair.Item2 < vVersion)
                    .OrderBy(pair => pair.Item2)
                    .LastOrDefault().str;
            }
        }

        if (foundVersion is null)
        {
            return (null, null);
        }
        else
        {
            return (Path.Combine(packageDir, foundVersion), foundVersion);
        }
    }

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
}
