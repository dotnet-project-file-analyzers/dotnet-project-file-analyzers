namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ReassignPropertiesWithDifferentValue() : MsBuildProjectFileAnalyzer(Rule.ReassignPropertiesWithDifferentValue)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var properties = new Dictionary<Key, Node>();

        foreach (var prop in context.Project
            .Walk()
            .Where(n => n.Parent is PropertyGroup))
        {
            if (properties.TryGetValue(Key.New(prop), out var previous)
                && Equals(previous.Val, prop.Val))
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
            properties[Key.New(prop)] = prop;
        }
    }

    private readonly record struct Key(string LocalName, string Constraints)
    {
        public static Key New(Node n) => new(n.LocalName, string.Join(";", n.Conditions()));
    }
}
