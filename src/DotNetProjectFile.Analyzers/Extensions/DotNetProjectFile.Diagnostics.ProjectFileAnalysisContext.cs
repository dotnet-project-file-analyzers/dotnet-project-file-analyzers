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

        /// <summary>Returns <see langword="false"/> only when the effective condition on <paramref name="node"/> (own + ancestor chain) evaluates definitively to <c>false</c>.</summary>
        public bool IsConditionEnabled(Node node)
        {
            var registry = new PropertyRegistry(node.Project.Path, context.File.Path);
            return node.AncestorsAndSelf()
                .Select(a => a.Condition)
                .OfType<string>()
                .All(c => ConditionEvaluator.Evaluate(c, registry) != false);
        }

        /// <summary>Yields nodes of type <typeparamref name="T"/> whose effective condition does not evaluate to <c>false</c>. Condition-aware drop-in for <c>context.File.Walk().OfType&lt;T&gt;()</c>.</summary>
        public IEnumerable<T> EnabledItems<T>() where T : Node
            => context.File.Walk().OfType<T>().Where(n => context.IsConditionEnabled(n));

        /// <summary>Yields typed properties of type <typeparamref name="T"/> whose effective condition does not evaluate to <c>false</c>. Condition-aware drop-in for <c>context.File.Properties&lt;T&gt;()</c>.</summary>
        public IEnumerable<T> EnabledProperties<T>() where T : Node
            => context.File.Properties<T>().Where(n => context.IsConditionEnabled(n));

        /// <summary>Returns the first typed property of type <typeparamref name="T"/> reachable via an enabled condition chain, or <see langword="null"/>. Condition-aware drop-in for <c>context.File.Property&lt;T&gt;()</c>.</summary>
        public T? EnabledProperty<T>() where T : Node
            => context.EnabledProperties<T>().FirstOrDefault();
    }

    /// <summary>Returns the last definitively-included <c>&lt;PropertyGroup&gt;</c> value for <paramref name="propertyName"/>, or <see langword="null"/> if undefined or only conditionally defined under unevaluable conditions.</summary>
    /// <remarks>
    /// Each group's condition is evaluated against a minimal <see cref="PropertyRegistry"/>
    /// (no user-defined lookup, breaking recursion), anchored to the file the group lives in
    /// so <c>$(MSBuildThisFileDirectory)</c> in an imported file resolves to the importing file.
    /// </remarks>
    [Pure]
    private static string? ScanUserDefinedProperty(MsBuildProject project, string propertyName)
        => project.PropertyGroups
            .Where(g => IsConditionTrue(g.Condition, RegistryFor(g, project)))
            .SelectMany(g => g.Children)
            .Where(c => IsConditionTrue(c.Condition, RegistryFor(c, project))
                && string.Equals(c.Element.Name.LocalName, propertyName, StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Element.Value)
            .LastOrDefault();

    [Pure]
    private static PropertyRegistry RegistryFor(Node node, MsBuildProject entry)
        => new(node.Project.Path, entry.Path);

    [Pure]
    private static bool IsConditionTrue(string? condition, PropertyRegistry registry)
        => string.IsNullOrEmpty(condition)
            || ConditionEvaluator.Evaluate(condition, registry) == true;
}
