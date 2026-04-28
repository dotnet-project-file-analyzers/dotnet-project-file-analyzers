namespace DotNetProjectFile.Diagnostics;

internal static class DotNetProjectFile
{
    private const string MsBuildThisFileDirectory = "$(MSBuildThisFileDirectory)";
    private const string MsBuildProjectDirectory = "$(MSBuildProjectDirectory)";

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

        /// <summary>
        /// Resolves the reserved MSBuild path properties in <paramref name="include"/>
        /// to the search-root directory and the literal portion of the include.
        /// </summary>
        /// <remarks>
        /// Supports <c>$(MSBuildThisFileDirectory)</c> (the directory of the file containing
        /// <paramref name="node"/>) and <c>$(MSBuildProjectDirectory)</c> (the directory of the
        /// entry-point project file). Includes containing other unresolved properties are returned
        /// unchanged, falling through to a literal lookup against the node's project directory.
        /// </remarks>
        public (IODirectory Root, string Literal) Resolve(Node node, string include)
            => (include.Contains(MsBuildThisFileDirectory), include.Contains(MsBuildProjectDirectory)) switch
            {
                (true, false) => (node.Project.Path.Directory, include.Replace(MsBuildThisFileDirectory, string.Empty)),
                (false, true) => (context.File.Path.Directory, include.Replace(MsBuildProjectDirectory, string.Empty)),
                _ => (node.Project.Path.Directory, include),
            };

        /// <summary>
        /// Enumerates files matching <paramref name="include"/> after resolving reserved
        /// MSBuild path properties. See <see cref="Resolve(Node, string)"/> for details.
        /// </summary>
        public IEnumerable<IOFile>? Files(Node node, string include)
        {
            var (root, literal) = context.Resolve(node, include);
            return root.Files(literal);
        }
    }
}
