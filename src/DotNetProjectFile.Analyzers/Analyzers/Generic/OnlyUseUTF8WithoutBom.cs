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
                foreach (var file in Walk(msbuild.Path.Directory)
                    .Select(f => new ProjectTextFile(f)))
                {
                    Register(new ProjectFileAnalysisContext<ProjectTextFile>(file, c.Compilation, c.Options, c.CancellationToken, c.ReportDiagnostic));
                }
            }
        });

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<ProjectTextFile> context)
    {
        if (!context.File.Path.Exists) { return; }

        using var reader = context.File.Path.TryOpenRead();

        var bom = new byte[3];

        _ = reader.Read(bom, 0, 3);

        if (bom[0] is 0xEF && bom[1] is 0xBB && bom[2] is 0xBF)
        {
            context.ReportDiagnostic(Descriptor, context.File.Path.AsLocation());
        }
    }

    private static IEnumerable<IOFile> Walk(IODirectory directory)
    {
        foreach (var file in directory.Files()?.Where(f => !Exclude(f)) ?? [])
        {
            yield return file;
        }

        foreach (var sub in directory.SubDirectories()?.Where(d => !Exclude(d)) ?? [])
        {
            foreach (var child in Walk(sub))
                yield return child;
        }
    }

    private static bool Exclude(IODirectory dir)
        => dir.Name
        is "bin"
        or "obj"
        or ".vs"
        or ".git"
        or ".nuget";

    private static bool Exclude(IOFile file)
        => file.Name.IsMatch("CompatibilitySuppressions.xml")
        || file.Extension is ".user";
}
