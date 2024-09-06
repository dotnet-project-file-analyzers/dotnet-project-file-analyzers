
namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProvideCompliantPackageIcon() : MsBuildProjectFileAnalyzer(Rule.ProvideCompliantPackageIcon)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.IsPackable() || context.Project.IsTestProject()) return;

        foreach (var icon in context.Project
            .SelfAndImports()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.PackageIcon)
            .Where(i => i.Value is { Length: > 0 }))
        {
            var info = Resolve(icon.Value!, context.Project);

            if (info is null)
            {
                continue;
            }

            if (info.Type != "PNG")
            {
                context.ReportDiagnostic(Descriptor, icon, icon.Value, "recommended to be a PNG");
            }
            if (info.Height != 128 || info.Width != 128)
            {
                context.ReportDiagnostic(Descriptor, icon, icon.Value, $"recommended to be 128x128 not {info.Width}x{info.Height}");
            }
        }
    }

    private static ImageInfo? Resolve(string iconValue, MsBuildProject project)
    {
        var file = project.Path.Directory.File(iconValue);

        if (!file.Exists && project
            .SelfAndImports()
            .SelectMany(p => p.ItemGroups)
            .SelectMany(g => g.BuildActions)
            .SelectMany(a => a.IncludeAndUpdate)
            .FirstOrDefault(a => a.EndsWith(iconValue)) is { } action)
        {
            file = project.Path.Directory.File(action);
        }

        try
        {
            using var stream = file.OpenRead();
            return ImageInfo.ReadPng(stream);
        }
        catch
        {
            return null;
        }
    }
}
