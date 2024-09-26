namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ItemGroupShouldBeUniform() : MsBuildProjectFileAnalyzer(Rule.ItemGroupShouldBeUniform)
{
    private static readonly IReadOnlyCollection<IReadOnlyCollection<Type>> Exceptions =
        [
            [
                typeof(Compile),
                typeof(EmbeddedResource),
                typeof(Content),
                typeof(None),
            ],
        ];

    private static readonly IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> Allowed
        = GenerateAllowList();

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var group in context.File.ItemGroups)
        {
            AnalyzeGroup(context, group);
        }
    }

    private void AnalyzeGroup(ProjectFileAnalysisContext context, ItemGroup group)
    {
        if (group.Children.FirstOrDefault()?.GetType() is not { } type)
        {
            return;
        }

        if (group.Children.Any(n => !CombinationIsAllowed(type, n.GetType())))
        {
            context.ReportDiagnostic(Descriptor, group);
        }
    }

    private static bool CombinationIsAllowed(Type first, Type second)
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

    private static IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> GenerateAllowList()
    {
        var result = new Dictionary<Type, IReadOnlyCollection<Type>>();

        foreach (var list in Exceptions)
        {
            var set = new HashSet<Type>(list);
            foreach (var item in set)
            {
                result[item] = set;
            }
        }

        return result;
    }
}
