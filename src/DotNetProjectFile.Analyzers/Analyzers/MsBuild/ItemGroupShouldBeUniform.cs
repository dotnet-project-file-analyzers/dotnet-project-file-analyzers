namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ItemGroupShouldBeUniform : MsBuildProjectFileAnalyzer
{
    private static readonly IReadOnlyCollection<IReadOnlyCollection<string>> Exceptions =
        [
            [
                "Compile",
                "EmbeddedResource",
                "Content",
                "None",
            ],
        ];

    private static readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> Allowed
        = GenerateAllowList();

    public ItemGroupShouldBeUniform() : base(Rule.ItemGroupShouldBeUniform) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var group in context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.ItemGroups))
        {
            AnalyzeGroup(context, group);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, ItemGroup group)
    {
        if (group.Children.FirstOrDefault()?.LocalName is not { } type)
        {
            return;
        }

        if (group.Children.Any(n => !CombinationIsAllowed(type, n.LocalName)))
        {
            context.ReportDiagnostic(Descriptor, group);
        }
    }

    private static bool CombinationIsAllowed(string first, string second)
    {
        if (first == second)
        {
            return true;
        }
        else if (Allowed.TryGetValue(first, out var allowed))
        {
            return allowed.Contains(second);
        }
        else
        {
            return false;
        }
    }

    private static IReadOnlyDictionary<string, IReadOnlyCollection<string>> GenerateAllowList()
    {
        var result = new Dictionary<string, IReadOnlyCollection<string>>();

        foreach (var list in Exceptions)
        {
            var set = new HashSet<string>(list);
            foreach (var item in set)
            {
                result[item] = set;
            }
        }

        return result;
    }
}
