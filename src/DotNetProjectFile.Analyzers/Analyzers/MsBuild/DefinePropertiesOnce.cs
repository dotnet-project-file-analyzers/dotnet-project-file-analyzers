namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePropertiesOnce() : MsBuildProjectFileAnalyzer(Rule.DefinePropertiesOnce)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var props = new HashSet<Node>(new PropertyComparer());

        foreach (var prop in context.Project.PropertyGroups.SelectMany(g => g.Children))
        {
            if (!props.Add(prop))
            {
                context.ReportDiagnostic(Descriptor, prop, prop.LocalName);
            }
        }
    }

    private sealed class PropertyComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node x, Node y)
            => Name(x) == Name(y)
            && Condition(x) == Condition(y);

        public int GetHashCode(Node obj)
            => (Name(obj).GetHashCode() * 13)
            ^ Condition(obj).GetHashCode();

        private static string Name(Node node)
            => node.LocalName == nameof(TargetFrameworks)
            ? nameof(TargetFramework)
            : node.LocalName;

        private static string Condition(Node node)
            => node.AncestorsAndSelf()
            .Select(n => n.Condition)
            .FirstOrDefault(c => c is { })
            ?? string.Empty;
    }
}
