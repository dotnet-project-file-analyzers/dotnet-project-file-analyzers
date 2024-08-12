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

    private record struct Reference(IOFile File, string Condition)
    {
        public static Reference Create(ProjectReference reference) => new(
            File: GetName(reference),
            Condition: string.Join(" And ", reference.Conditions()));

        private static IOFile GetName(ProjectReference reference)
            => reference.Include is null
            ? IOFile.Empty
            : reference.Project.Path.Directory.File(reference.Include);
    }
}
