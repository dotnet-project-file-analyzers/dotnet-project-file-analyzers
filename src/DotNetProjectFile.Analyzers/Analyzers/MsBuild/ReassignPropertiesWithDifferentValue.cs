namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ReassignPropertiesWithDifferentValue() : MsBuildProjectFileAnalyzer(Rule.ReassignPropertiesWithDifferentValue)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var properties = new Dictionary<Prop, Node>();

        foreach (var prop in context.File
            .Walk()
            .Where(n => n.Parent is PropertyGroup))
        {
            var key = Prop.New(prop);

            if (properties.TryGetValue(key, out var previous) && Equals(previous.Val, prop.Val))
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
            properties[key] = prop;
        }
    }

    private readonly record struct Prop(string Name, string Condition)
    {
        public static Prop New(Node n) => new(n.LocalName,  Conditions.ToString(n));
    }
}
