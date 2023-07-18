namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class IncludePackageReferencesOnce : MsBuildProjectFileAnalyzer
{
    public IncludePackageReferencesOnce() : base(Rule.IncludePackageReferencesOnce) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var lookup = new Dictionary<Reference, PackageReference>();

        foreach (var reference in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(i => i.PackageReferences))
        {
            var key = Reference.Create(reference);

            if (lookup.TryGetValue(key, out var existing) && !IsOverride(existing, reference))
            {
                context.ReportDiagnostic(Descriptor, reference, key.Name);
            }
            else
            {
                lookup[key] = reference;
            }
        }
    }

    private static bool IsOverride(PackageReference existing, PackageReference reference)
        => existing.Project != reference.Project
        && reference.Update is { Length: > 0 };

    private record struct Reference(string Name, string Condition)
    {
        public static Reference Create(PackageReference reference) => new(
            Name: reference.Include ?? reference.Update ?? string.Empty,
            Condition: string.Join(" And ", reference.Conditions()));
    }
}
