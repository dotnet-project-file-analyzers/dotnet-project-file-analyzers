using System.IO;

namespace DotNetProjectFile.Diagnostics;

internal static class DotNetProjectFile
{
    extension(ProjectFileAnalysisContext context)
    {
        /// <summary>Indicates that CPM is enabled.</summary>
        public bool ManagePackageVersionsCentrally
            => context.Props.ManagePackageVersionsCentrally
            ?? context.File.Property<ManagePackageVersionsCentrally>()?.Value
            ?? MsBuildDefaults.ManagePackageVersionsCentrally;

        /// <summary>Indicates that the .NET analyzers are enabled.</summary>
        public bool EnableNETAnalyzers
            => context.Props.EnableNETAnalyzers
            ?? context.File.Property<EnableNETAnalyzers>()?.Value
            ?? MsBuildDefaults.EnableNETAnalyzers;

        /// <summary>Resolves MSBuild property references in <paramref name="include"/> to a (search root, literal) pair.</summary>
        /// <remarks>
        /// Relative paths and inputs with unresolved properties anchor the search root at
        /// <c>context.File.Path.Directory</c> (the consuming project), matching MSBuild's
        /// item-evaluation rule for paths declared in imported files.
        /// </remarks>
        public (IODirectory Root, string Literal) Resolve(Node node, string include)
        {
            var (substituted, unresolved) = context.Substitute(node, include);

            if (unresolved.Count == 0 && Path.IsPathRooted(substituted))
            {
                var lastSep = substituted.LastIndexOfAny(['/', '\\']);
                if (lastSep > 0 && lastSep < substituted.Length - 1)
                {
                    return (IODirectory.Parse(substituted[..lastSep]), substituted[(lastSep + 1)..]);
                }
            }

            return (context.File.Path.Directory, substituted);
        }

        /// <summary>Expands MSBuild <c>$(...)</c> references in <paramref name="include"/>, returning the substituted string and the names of any unresolved properties.</summary>
        public SubstitutionResult Substitute(Node node, string include)
        {
            var registry = new PropertyRegistry(
                node.Project.Path,
                context.File.Path,
                name => ScanUserDefinedProperty(context.File, name));
            return registry.Substitute(include);
        }

        /// <summary>Enumerates files matching <paramref name="include"/> after MSBuild property resolution.</summary>
        public IEnumerable<IOFile>? Files(Node node, string include)
        {
            var (root, literal) = context.Resolve(node, include);
            return root.Files(literal);
        }
    }

    /// <summary>Returns the last unconditionally-included <c>&lt;PropertyGroup&gt;</c> value for <paramref name="propertyName"/>, or <see langword="null"/> if undefined.</summary>
    /// <remarks>
    /// Conditional <c>&lt;PropertyGroup&gt;</c> and conditional individual properties are skipped:
    /// the conservative choice without a condition evaluator is to scan only the
    /// definitively-included groups.
    /// </remarks>
    [Pure]
    private static string? ScanUserDefinedProperty(MsBuildProject project, string propertyName)
        => project.PropertyGroups
            .Where(g => string.IsNullOrEmpty(g.Condition))
            .SelectMany(g => g.Children)
            .Where(c => string.IsNullOrEmpty(c.Condition)
                && string.Equals(c.Element.Name.LocalName, propertyName, StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Element.Value)
            .LastOrDefault();
}
