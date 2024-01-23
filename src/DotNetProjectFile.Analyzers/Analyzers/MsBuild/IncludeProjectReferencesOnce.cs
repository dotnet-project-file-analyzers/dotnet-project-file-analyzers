using System.IO;
using ProjectReference = DotNetProjectFile.MsBuild.ProjectReference;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludeProjectReferencesOnce() : MsBuildProjectFileAnalyzer(Rule.IncludeProjectReferencesOnce)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var lookup = new Dictionary<Reference, ProjectReference>();

        foreach (var reference in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(i => i.ProjectReferences)
            .Where(r => r.Include is not null))
        {
            var key = Reference.Create(reference);

            if (lookup.TryGetValue(key, out var existing))
            {
                context.ReportDiagnostic(Descriptor, reference, reference.Include);
            }
            else
            {
                lookup[key] = reference;
            }
        }
    }

    private record struct Reference(string Name, string Condition)
    {
        public static Reference Create(ProjectReference reference) => new(
            Name: GetName(reference),
            Condition: string.Join(" And ", reference.Conditions()));

        private static string GetName(ProjectReference reference)
        {
            if (reference.Include is null)
            {
                return string.Empty;
            }

            // Paths are resolved in order to find duplicate paths that are not lexically equal.
            var root = reference.Project;
            var rootDir = Path.GetDirectoryName(root.Path.FullName);
            var unixFriendly = reference.Include.Replace(@"\", "/"); // Make sure it works on Unix based systems.
            var combined = Path.Combine(rootDir, unixFriendly);
            var fullPath = Path.GetFullPath(combined);
            var lower = fullPath.ToLowerInvariant(); // MSBuild is case insensitive on Windows.
            return lower;
        }
    }
}
