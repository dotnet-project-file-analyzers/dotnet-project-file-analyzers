using DotNetProjectFile.Resx;

namespace DotNetProjectFile.Analyzers.Resx;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EmbedValidResourceFiles : ResourceFileAnalyzer
{
    public EmbedValidResourceFiles() : base(Rule.EmbedValidResourceFiles) { }

    protected override void Register(ResourceFileAnalysisContext context)
    {
        var resource = context.File;

        if (!resource.IsXml)
        {
            context.ReportDiagnostic(Descriptor, resource, "contains no XML.");
        }
        else
        {
            if (resource.Headers.None(h => Matches(h, "resmimetype", t => t == "text/microsoft-resx")))
            {
                context.ReportDiagnostic(Descriptor, resource, @"misses <resheader name=""resmimetype""> with value ""text/microsoft-resx"".");
            }
            if (resource.Headers.None(h => Matches(h, "reader", t => t.StartsWith("System.Resources.ResXResourceReader"))))
            {
                context.ReportDiagnostic(Descriptor, resource, @"misses <resheader name=""reader""> with value ""System.Resources.ResXResourceReader"".");
            }
            if (resource.Headers.None(h => Matches(h, "writer", t => t.StartsWith("System.Resources.ResXResourceWriter"))))
            {
                context.ReportDiagnostic(Descriptor, resource, @"misses <resheader name=""writer""> with value ""System.Resources.ResXResourceWriter"".");
            }
        }
    }

    private static bool Matches(ResHeader header, string name, Func<string, bool> match)
        => header.Name == name
        && header.Value?.Text is { Length: > 0 } text
        && match(text);
}
