using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.Generic;

/// <summary>Implements <see cref="Rule.OnlyUseUTF8WithoutBom"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OnlyUseUTF8WithoutBom() : ProjectFileAnalyzer<ProjectTextFile>(Rule.OnlyUseUTF8WithoutBom)
{
    protected override void Register(AnalysisContext context)
    {
        context.RegisterCompilationAction(c =>
        {
            if (ProjectFiles.Global.UpdateMsBuildProject(c) is { } msbuild)
            {
                foreach (var file in msbuild.Path.Directory.Files("**/*")?
                    .Where(Include)
                    .Select(f => new ProjectTextFile(f))
                    .Where(f => !f.IsAdditional(c.Options.AdditionalFiles)) ?? [])
                {
                    Register(new ProjectFileAnalysisContext<ProjectTextFile>(file, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });

        context.RegisterAdditionalFileAction(c =>
        {
            Register(new ProjectFileAnalysisContext<ProjectTextFile>(new(IOFile.Parse(c.AdditionalFile.Path)), c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
        });
    }

    protected override void Register(ProjectFileAnalysisContext<ProjectTextFile> context)
    {
        if (!context.File.Path.Exists) { return; }

        using var reader = context.File.Path.TryOpenRead();

        var bom = new byte[3];

        _ = reader.Read(bom, 0, 3);

        if (bom[0] is 0xEF && bom[1] is 0xBB && bom[2] is 0xBF)
        {
            var span = new TextSpan(0, 1);
            var line = new LinePositionSpan(new(0, 0), new(0, 1));
            context.ReportDiagnostic(Descriptor, Location.Create(context.File.Path.ToString(), span, line));
        }
    }

    private static bool Include(IOFile file)
    {
        var path = file.ToString("/");

        return !path.Contains("/bin/")
            && !path.Contains("/obj/")
            && !file.Name.IsMatch("CompatibilitySuppressions.xml");
    }
}
