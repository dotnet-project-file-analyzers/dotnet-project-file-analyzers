namespace DotNetProjectFile.NuGet;

public sealed class Packages : IReadOnlyCollection<Package>
{
    public static readonly Packages All = new(

        new Analyzer("AsyncFixer"),
        new Analyzer("DotNetProjectFile.Analyzers"),
        new Analyzer("Microsoft.AspNetCore.Components.Analyzers"),
        new Analyzer("Microsoft.CodeAnalysis.NetAnalyzers"),
        new Analyzer("Qowaiv.Analyzers.CShar"),
        new Analyzer("SonarAnalyzer.CSharp", language: LanguageNames.CSharp),
        new Analyzer("SonarAnalyzer.VisualBasic", language: LanguageNames.VisualBasic),
        new Analyzer("StyleCop.Analyzers"),

        new Analyzer("Ardalis.ApiEndpoints.CodeAnalyzers", "Ardalis.ApiEndpoints"),
        new Analyzer("FakeItEasy.Analyzer.CSharp", "FakeItEasy", LanguageNames.CSharp),
        new Analyzer("FakeItEasy.Analyzer.VisualBasic", "FakeItEasy", LanguageNames.VisualBasic),
        new Analyzer("FluentAssertions.Analyzers", "FluentAssertions"),
        new Analyzer("Libplanet.Analyzers", "Libplanet"),
        new Analyzer("Lucene.Net.Analysis.Common", "Lucene.Net"),
        new Analyzer("MassTransit.Analyzers", "MassTransit"),
        new Analyzer("MessagePackAnalyzer", "MessagePack"),
        new Analyzer("MessagePipe.Analyzer", "MessagePipe"),
        new Analyzer("Microsoft.AspNetCore.Components.Analyzers", "Microsoft.AspNetCore"),
        new Analyzer("Microsoft.Azure.Functions.Analyzers", "Microsoft.Azure.Functions"),
        new Analyzer("Microsoft.CodeAnalysis.Analyzers", "Microsoft.CodeAnalysis"),
        new Analyzer("Microsoft.EntityFrameworkCore.Analyzers", "Microsoft.EntityFrameworkCore"),
        new Analyzer("Microsoft.ServiceHub.Analyzers", "Microsoft.ServiceHub"),
        new Analyzer("MongoDB.Analyzer", "MongoDB"),
        new Analyzer("Moq.Analyzers", "Moq"),
        new Analyzer("NSubstitute.Analyzers.CSharp", "NSubstitute", LanguageNames.CSharp),
        new Analyzer("NSubstitute.Analyzers.VisualBasic", "NSubstitute", LanguageNames.VisualBasic),
        new Analyzer("NUnit.Analyzers", "NUnit"),
        new Analyzer("RuntimeContracts.Analyzer", "RuntimeContracts"),
        new Analyzer("SerilogAnalyzer", "Serilog"),
        new Analyzer("xunit.analyzers", "xunit"),
        new Analyzer("ZeroFormatter.Analyzer", "ZeroFormatter"),

        new Package("coverlet.collector", isPrivateAsset: true),
        new Package("coverlet.msbuild", isPrivateAsset: true),
        new Package("NUnit3TestAdapter", isPrivateAsset: true),
        new Package("Polyfill", isPrivateAsset: true),
        new Package("PolySharp", isPrivateAsset: true));

    private readonly Dictionary<string, Package> items;

    private Packages(params Package[] packages)
    {
        items = packages.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);
    }

    public int Count => items.Count;

    public Package? TryGet(string? name)
        => name is { Length: > 0 } && items.TryGetValue(name, out var package)
            ? package
            : null;

    public IEnumerator<Package> GetEnumerator() => items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
