using System.IO;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.MsBuild;

/// <summary>Resolves MSBuild property names to values and substitutes <c>$(Property)</c> references in strings.</summary>
/// <remarks>
/// <c>MSBuildThisFile*</c> resolves against <paramref name="containingFile"/> (the file the
/// reference textually appears in); <c>MSBuildProject*</c> resolves against <paramref name="entryFile"/>
/// (the project being analysed). Reserved properties take precedence over the
/// <paramref name="userDefinedLookup"/> delegate. Property names are matched case-insensitively.
/// <c>$(MSBuildThisFileDirectory)</c> includes a trailing separator;
/// <c>$(MSBuildProjectDirectory)</c> does not, matching MSBuild.
/// </remarks>
public sealed class PropertyRegistry(
    IOFile containingFile,
    IOFile entryFile,
    Func<string, string?>? userDefinedLookup = null)
{
    /// <summary>Matches <c>$(Property)</c> with a word-character property name; malformed references (no close paren, empty name, dotted name) pass through as literals.</summary>
    private static readonly Regex PropertyReference = new(
        @"\$\((?<Property>\w+)\)",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    /// <summary>Path-derivable reserved MSBuild properties, keyed case-insensitively.</summary>
    private static readonly IReadOnlyDictionary<string, Func<PropertyRegistry, string?>> Reserved =
        new Dictionary<string, Func<PropertyRegistry, string?>>(StringComparer.OrdinalIgnoreCase)
        {
            ["MSBuildThisFile"] = static r => r.ContainingFile.Name,
            ["MSBuildThisFileDirectory"] = static r => DirectoryWithSeparator(r.ContainingFile),
            ["MSBuildThisFileDirectoryNoRoot"] = static r => DirectoryNoRootWithSeparator(r.ContainingFile),
            ["MSBuildThisFileExtension"] = static r => r.ContainingFile.Extension,
            ["MSBuildThisFileFullPath"] = static r => r.ContainingFile.ToString(),
            ["MSBuildThisFileName"] = static r => r.ContainingFile.NameWithoutExtension,

            ["MSBuildProjectFile"] = static r => r.EntryFile.Name,
            ["MSBuildProjectDirectory"] = static r => DirectoryWithoutSeparator(r.EntryFile),
            ["MSBuildProjectDirectoryNoRoot"] = static r => DirectoryNoRootWithoutSeparator(r.EntryFile),
            ["MSBuildProjectExtension"] = static r => r.EntryFile.Extension,
            ["MSBuildProjectFullPath"] = static r => r.EntryFile.ToString(),
            ["MSBuildProjectName"] = static r => r.EntryFile.NameWithoutExtension,
        };

    private readonly IOFile ContainingFile = containingFile;
    private readonly IOFile EntryFile = entryFile;
    private readonly Func<string, string?>? UserDefinedLookup = userDefinedLookup;

    /// <summary>Returns the resolved value of <paramref name="propertyName"/>, or <see langword="null"/> if neither reserved nor surfaced by the user-defined lookup.</summary>
    [Pure]
    public string? Lookup(string propertyName)
        => Reserved.TryGetValue(propertyName, out var resolve)
            ? resolve(this)
            : UserDefinedLookup?.Invoke(propertyName);

    /// <summary>Expands every <c>$(Property)</c> reference in <paramref name="input"/>; unknown references are left literal and reported via <see cref="SubstitutionResult.Unresolved"/>.</summary>
    [Pure]
    public SubstitutionResult Substitute(string input)
    {
        if (string.IsNullOrEmpty(input)) return new(input, []);

        var unresolved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var substituted = PropertyReference.Replace(input, match =>
        {
            var name = match.Groups["Property"].Value;
            var value = Lookup(name);
            if (value is null)
            {
                unresolved.Add(name);
                return match.Value;
            }
            return value;
        });

        return new(substituted, [.. unresolved]);
    }

    [Pure]
    private static string DirectoryWithSeparator(IOFile file)
        => file.HasValue ? file.Directory.ToString() + IOPath.Separator : string.Empty;

    [Pure]
    private static string DirectoryWithoutSeparator(IOFile file)
        => file.HasValue ? file.Directory.ToString() : string.Empty;

    [Pure]
    private static string DirectoryNoRootWithSeparator(IOFile file)
    {
        var noRoot = StripRoot(file);
        return noRoot.Length == 0 ? string.Empty : noRoot + IOPath.Separator;
    }

    [Pure]
    private static string DirectoryNoRootWithoutSeparator(IOFile file)
        => StripRoot(file);

    [Pure]
    private static string StripRoot(IOFile file)
    {
        if (!file.HasValue) return string.Empty;
        var directory = file.Directory.ToString();
        var root = Path.GetPathRoot(directory);
        return string.IsNullOrEmpty(root)
            ? directory
            : directory[root!.Length..].TrimStart('/', '\\');
    }
}

/// <summary>Outcome of <see cref="PropertyRegistry.Substitute"/>.</summary>
/// <param name="Substituted">The input with known <c>$(...)</c> references replaced; unknown references stay literal.</param>
/// <param name="Unresolved">Distinct unresolved property names (case-insensitive); empty when every reference was resolved.</param>
public readonly record struct SubstitutionResult(string Substituted, IReadOnlyList<string> Unresolved);
