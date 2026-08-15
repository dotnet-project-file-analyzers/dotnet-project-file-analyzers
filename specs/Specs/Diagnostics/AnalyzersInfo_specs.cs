using DotNetProjectFile.Diagnostics;
using System.IO;

namespace Diagnostics.AnalyzersInfo_specs;

public class Embedded
{
    [Test]
    public void Contains_rules()
    {
        var info = DiagnosticCollection.Embedded();
        info.Packages.Should().AllSatisfy(p => p.Rules.Should().NotBeEmpty(because: p.Id));
        info.Count.Should().Be(7354);
    }

    [Test]
    public void Contains_packages()
    {
        var info = DiagnosticCollection.Embedded();
        var packages = info.Packages.Select(p => p.Id);

#if DEBUG
        Console.WriteLine(string.Join(",\n", packages.Select(p => $"\"{p}\"")));
#endif

        packages.Should().BeEquivalentTo(
            "Agoda.Analyzers",
            "Akka.Analyzers",
            "Apex.Analyzers.Immutable",
            "Ardalis.ApiEndpoints.CodeAnalyzers",
            "AsyncFixer",
            "AutoMapperAnalyzer.Analyzers",
            "Bit.CodeAnalyzers",
            "BlowinCleanCode",
            "ClrHeapAllocationAnalyzer",
            "CodeCracker.CSharp",
            "CodeCracker.VisualBasic",
            "ConfigureAwaitChecker.Analyzer",
            "CSharpGuidelinesAnalyzer",
            "D2L.CodeStyle.Analyzers",
            "DotNetProjectFile.Analyzers",
            "ErrorProne.NET",
            "Faithlife.Analyzers",
            "FakeItEasy.Analyzer.CSharp",
            "FakeItEasy.Analyzer.VisualBasic",
            "FluentAssertions.Analyzers",
            "FunFair.CodeAnalysis",
            "GlobalUsingsAnalyzer",
            "Gu.Analyzers",
            "IDisposableAnalyzers",
            "Libplanet.Analyzers",
            "Marten.Analyzers",
            "MassTransit.Analyzers",
            "Menees.Analyzers",
            "MessagePackAnalyzer",
            "MessagePipe.Analyzer",
            "Meziantou.Analyzer",
            "Microsoft.AspNetCore.Components.Analyzers",
            "Microsoft.Azure.Functions.Analyzers",
            "Microsoft.Azure.Functions.Worker.Sdk.Analyzers",
            "Microsoft.CodeAnalysis.Analyzers",
            "Microsoft.CodeAnalysis.CSharp",
            "Microsoft.CodeAnalysis.CSharp.CodeStyle",
            "Microsoft.CodeAnalysis.NetAnalyzers",
            "Microsoft.CodeAnalysis.VisualBasic",
            "Microsoft.CodeAnalysis.VisualBasic.CodeStyle",
            "Microsoft.EntityFrameworkCore.Analyzers",
            "Microsoft.ServiceHub.Analyzers",
            "Microsoft.VisualStudio.SDK.Analyzers",
            "Microsoft.VisualStudio.Threading.Analyzers",
            "MongoDB.Analyzer",
            "Moq.Analyzers",
            "MSTest.Analyzers",
            "NSubstitute.Analyzers.CSharp",
            "NSubstitute.Analyzers.VisualBasic",
            "NUnit.Analyzers",
            "Octopus.Nevermore.Analyzers",
            "Philips.CodeAnalysis.DuplicateCodeAnalyzer",
            "Philips.CodeAnalysis.MaintainabilityAnalyzers",
            "Philips.CodeAnalysis.MoqAnalyzers",
            "Philips.CodeAnalysis.MsTestAnalyzers",
            "Qowaiv.Analyzers.CSharp",
            "ReflectionAnalyzers",
            "RG.CodeAnalyzer",
            "Roslynator.Analyzers",
            "Roslynator.CodeAnalysis.Analyzers",
            "Roslynator.Formatting.Analyzers",
            "RuntimeContracts.Analyzer",
            "SerilogAnalyzer",
            "SharpSource",
            "SonarAnalyzer.CSharp",
            "SonarAnalyzer.VisualBasic",
            "Spectre.Console.Analyzer",
            "StructuredLogging.Analyzers",
            "StyleCop.Analyzers.Unstable",
            "Text.Analyzers",
            "Uno.MonoAnalyzers",
            "Wintellect.Analyzers",
            "xunit.analyzers",
            "ZeroFormatter.Analyzer");
    }
}

[Explicit("Long running process that alters the embedded resource")]
public class Collects
{
    [Test]
    public async Task New_rules()
    {
        var info = DiagnosticCollection.Embedded();
        info = await DiagnosticCollector.Collect(info);

        var file = new DirectoryInfo("../../../../../src/DotNetProjectFile.Diagnostics/Data/DiagnosticCollection.json");
        using var stream = new FileStream(file.FullName, FileMode.Create);

        info.Save(stream);
        info.Packages.Should().AllSatisfy(p => p.Rules.Should().NotBeEmpty(p.Id));
    }
}
