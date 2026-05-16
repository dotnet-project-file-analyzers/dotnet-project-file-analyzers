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

        /// <summary>Returns <c> (resolved to '&lt;path&gt;')</c> when <paramref name="include"/> has resolvable MSBuild property references; empty otherwise.</summary>
        /// <remarks>
        /// The resolved path is project-relative (anchored at <c>context.File.Path.Directory</c>) so
        /// diagnostic messages stay deterministic across machines and operating systems. Returns empty
        /// when the literal does not change under substitution, or when any property reference is
        /// unresolved (those cases are surfaced by the path-existence rules and by <c>Proj9000</c>).
        /// </remarks>
        public string ResolvedSuffix(Node node, string include)
        {
            if (string.IsNullOrEmpty(include)) return string.Empty;
            var (substituted, unresolved) = context.Substitute(node, include);

            // Suppress the suffix on partial substitution: showing a half-resolved path
            // next to half-literal $(...) tokens would mix idioms and obscure the
            // unresolved-property diagnostic that Proj9000 already surfaces separately.
            if (unresolved.Count > 0 || string.Equals(substituted, include, StringComparison.Ordinal)) return string.Empty;

            var relative = ProjectRelative(context.File.Path.Directory, substituted);
            return $" (resolved to '{relative}')";
        }
    }

    /// <summary>Returns <paramref name="substituted"/> rewritten as a forward-slash path relative to <paramref name="projectDirectory"/>; the unchanged input when not under the project.</summary>
    /// <remarks>
    /// <c>OrdinalIgnoreCase</c> defends against the Windows case-insensitive filesystem;
    /// on Linux MSBuild already resolves paths with exact casing.
    /// </remarks>
    [Pure]
    private static string ProjectRelative(IODirectory projectDirectory, string substituted)
    {
        var normalized = substituted.Replace('\\', '/');
        var projectDir = projectDirectory.ToString().Replace('\\', '/').TrimEnd('/') + '/';
        return normalized.StartsWith(projectDir, StringComparison.OrdinalIgnoreCase)
            ? normalized[projectDir.Length..]
            : normalized;
    }

    /// <summary>Returns the last unconditionally-included <c>&lt;PropertyGroup&gt;</c> value for <paramref name="propertyName"/>, or <see langword="null"/> if undefined.</summary>
    /// <remarks>
    /// <c>&lt;PropertyGroup&gt;</c> elements nested inside a <c>&lt;Target&gt;</c> are skipped:
    /// MSBuild evaluates those only when the target runs, so their values are not part of the
    /// statically-analyzable property set. This mirrors the <c>Target</c> exclusion applied in
    /// <c>MsBuildProject.Read&lt;TNode&gt;</c>.
    /// Conditional <c>&lt;PropertyGroup&gt;</c> and conditional individual properties are also
    /// skipped: the conservative choice without a condition evaluator is to scan only the
    /// definitively-included groups.
    /// </remarks>
    [Pure]
    private static string? ScanUserDefinedProperty(MsBuildProject project, string propertyName)
        => project.PropertyGroups
            .Where(g => !IsInsideTarget(g))
            .Where(g => string.IsNullOrEmpty(g.Condition))
            .SelectMany(g => g.Children)
            .Where(c => string.IsNullOrEmpty(c.Condition)
                && string.Equals(c.Element.Name.LocalName, propertyName, StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Element.Value)
            .LastOrDefault();

    [Pure]
    private static bool IsInsideTarget(Node node)
    {
        for (var ancestor = node.Parent; ancestor is not null; ancestor = ancestor.Parent)
        {
            if (ancestor is Target) return true;
        }
        return false;
    }
}
