namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludePackageReferencesOnce() : MsBuildProjectFileAnalyzer(Rule.IncludePackageReferencesOnce)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var references = new Dictionary<Reference, PackageReference>();

        foreach (var reference in context.File
            .Walk()
            .OfType<PackageReference>()
            .Where(p => p.Include is { Length: > 0 }))
        {
            var key = Reference.New(reference);

            if (references.TryGetValue(key, out var existing) && !IsOverride(existing, reference))
            {
                context.ReportDiagnostic(Descriptor, reference, key.Name);
            }
            else
            {
                references[key] = reference;
            }
        }
    }

    private static bool IsOverride(PackageReference existing, PackageReference reference)
        => existing.Project != reference.Project
        && reference.Update is { Length: > 0 };

    private record struct Reference(string Name, string Condition)
    {
        public static Reference New(PackageReference r) => new(r.Include!, Conditions.ToString(r));
    }
}
