using DotNetProjectFile.MsBuild;
using DotNetProjectFile.NuGet.Packaging;
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

        return new()
        {
            Name = name,
            Version = versionDir.Name,
            HasAnalyzerDll = HasDllFiles("analyzers"),
            HasRuntimeDll = HasDllFiles("lib") || HasDllFiles("runtimes"),
            NuSpecFile = TryLoadNuSpecFile(versionDir),
        };

        bool HasDllFiles(string subDir)
        {
            var dir = versionDir.SubDirectory(subDir);
            if (!dir.Exists)
            {
                return false;
            }

            return dir.Files("./**/*.dll").Any();
        }
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

        IODirectory? foundVersion;
        if (version is null)
        {
            // Default to highest found package version if the input version was gibberish.
            foundVersion = packageDir.SubDirectories()
                .OrderBy(d => TryParseVersion(d.Name))
                .LastOrDefault();
        }
        else
        {
            var pairs = packageDir.SubDirectories()
                .Select(d => (d, TryParseVersion(d.Name)))
                .ToArray();

            // Pick nearest version that is higher than the current version if available.
            foundVersion = pairs
                .Where(pair => pair.Item2 > version)
                .OrderBy(pair => pair.Item2)
                .FirstOrDefault().d;

            if (foundVersion is null)
            {
                // Pick nearest version that is lower than the current version if available.
                foundVersion = pairs
                    .Where(pair => pair.Item2 < version)
                    .OrderBy(pair => pair.Item2)
                    .LastOrDefault().d;
            }
        }

        return foundVersion;
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

    private static NuSpecFile? TryLoadNuSpecFile(IODirectory directory)
    {
        if(directory.Files("*.nuspec")?.FirstOrDefault() is { HasValue: true} nuspec)
        {
            try
            {
                using var stream = nuspec.OpenRead();
                return NuSpecFile.Load(stream);
            }
            catch { }
        }
        return null;
    }
}
