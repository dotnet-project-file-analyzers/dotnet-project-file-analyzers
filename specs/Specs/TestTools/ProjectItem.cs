using Buildalyzer;

namespace Specs.TestTools;

public sealed record ProjectItem
{
    public required string ItemSpec { get; init; }

    public Dictionary<string, string> Metadata { get; init; } = [];

    public override string ToString()
        => $"ItemSpec = {ItemSpec}, Metadata = {{ {string.Join(", ", Metadata.Select(x => $"{x.Key} = {x.Value}"))} }}";

    internal static void Generate(IEnumerable<IProjectItem> content)
    {
        foreach (var item in content)
        {
            Console.WriteLine("new ProjectItem\n{");
            Console.WriteLine($"""    ItemSpec = "{item.ItemSpec.Replace('\\', '/')}", """);
            Console.WriteLine("    Metadata = new Meta\n    {");
            foreach (var (key, value) in item.Metadata)
            {
                Console.WriteLine($"""        {key} = "{value}",""");
            }
            Console.WriteLine("    },\n},");
        }
    }

    public record Meta
    {
        private readonly Dictionary<string, string> Lookup = [];

        public string? AnalyzerType { get => Get(nameof(AnalyzerType)); init => Set(nameof(AnalyzerType), value); }
        public string? CopyToOutputDirectory { get => Get(nameof(CopyToOutputDirectory)); init => Set(nameof(CopyToOutputDirectory), value); }
        public string? Link { get => Get(nameof(Link)); init => Set(nameof(Link), value); }
        public string? Visible { get => Get(nameof(Visible)); init => Set(nameof(Visible), value); }
        public string? SonarQubeContent { get => Get(nameof(SonarQubeContent)); init => Set(nameof(SonarQubeContent), value); }

        public string? PackagePath { get => Get(nameof(PackagePath)); init => Set(nameof(PackagePath), value); }
        public string? Pack { get => Get(nameof(Pack)); init => Set(nameof(Pack), value); }

        private string? Get(string key) => Lookup.TryGetValue(key, out var val) ? val : null;

        private void Set(string key, string? value)
        {
            if (value is null) Lookup.Remove(key);
            else Lookup[key] = value;
        }

        public static implicit operator Dictionary<string, string>(Meta metadata) => metadata.Lookup;
    }
}

internal static class ProjectItemExtensions
{
    extension(IReadOnlyDictionary<string, IProjectItem[]> items)
    {
        public IEnumerable<IProjectItem> OfType(string name)
            => (items.TryGetValue(name, out var selection) ? selection : [])
            .OrderBy(x => x.ItemSpec);
    }
}
