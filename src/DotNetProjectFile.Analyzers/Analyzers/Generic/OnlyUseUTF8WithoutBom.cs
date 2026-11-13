namespace DotNetProjectFile.Analyzers.Generic;

/// <summary>Implements <see cref="Rule.OnlyUseUTF8WithoutBom"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class OnlyUseUTF8WithoutBom() : ProjectFileAnalyzer<ProjectTextFile>(Rule.OnlyUseUTF8WithoutBom)
{
    /// <inheritdoc />
    protected override void Register(AnalysisContext context)
        => context.RegisterCompilationAction(c =>
        {
            if (ProjectFiles.Global.UpdateMsBuildProject(c) is { } msbuild)
            {
                foreach (var context in new IOFiles(msbuild.Path.Directory.ToString(), IOFiles.DefaultIgnores)
                    .Where(HasBom)
                    .Select(f => new ProjectTextFile(IOFile.Parse(f.FullName))))
                {
                    Register(new ProjectFileAnalysisContext<ProjectTextFile>(context, AnalyzerType.None, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<ProjectTextFile> context)
        => context.ReportDiagnostic(Descriptor, context.File.Path.AsLocation());

    private static bool HasBom(FileRef file)
    {
        try
        {
            var bom = new byte[3];
            using var stream = file.OpenRead();
            _ = stream.Read(bom, 0, 3);

            return bom[0] is 0xEF
                && bom[1] is 0xBB
                && bom[2] is 0xBF
                && !file.FullName.EndsWith("CompatibilitySuppressions.xml")
                && !file.FullName.EndsWith(".user");
        }
        catch
        {
            return false;
        }
    }
}
