using ProjectReference = DotNetProjectFile.MsBuild.ProjectReference;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludeProjectReferencesOnce() : MsBuildProjectFileAnalyzer(Rule.IncludeProjectReferencesOnce)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var references = new Dictionary<Reference, ProjectReference>();

        foreach (var reference in context.File
            .Walk()
            .OfType<ProjectReference>()
            .Where(r => r.Include is { Length: > 0 }))
        {
            var key = Reference.New(reference);

            if (references.TryGetValue(key, out var existing))
            {
                context.ReportDiagnostic(Descriptor, reference, reference.Include);
            }
            else
            {
                references[key] = reference;
            }
        }
    }

    private record struct Reference(IOFile File, string Condition)
    {
        public static Reference New(ProjectReference r)
            => new(r.Project.Path.Directory.File(r.Include!), Conditions.ToString(r));
    }
}
