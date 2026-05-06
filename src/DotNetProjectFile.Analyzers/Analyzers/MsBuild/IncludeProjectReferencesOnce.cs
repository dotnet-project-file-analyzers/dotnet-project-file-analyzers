using ProjectReference = DotNetProjectFile.MsBuild.ProjectReference;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.IncludeProjectReferencesOnce"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludeProjectReferencesOnce() : MsBuildProjectFileAnalyzer(Rule.IncludeProjectReferencesOnce)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var references = new Dictionary<Reference, ProjectReference>();

        foreach (var reference in context.EnabledItems<ProjectReference>()
            .Where(r => r.Include is { Length: > 0 }))
        {
            var (root, literal) = context.Resolve(reference, reference.Include!);
            var key = new Reference(root.File(literal), Conditions.ToString(reference));

            if (references.ContainsKey(key))
            {
                context.ReportDiagnostic(Descriptor, reference, reference.Include);
            }
            else
            {
                references[key] = reference;
            }
        }
    }

    private record struct Reference(IOFile File, string Condition);
}
