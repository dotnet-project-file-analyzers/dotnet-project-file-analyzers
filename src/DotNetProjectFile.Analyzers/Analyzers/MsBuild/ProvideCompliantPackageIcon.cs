namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProvideCompliantPackageIcon() : MsBuildProjectFileAnalyzer(Rule.ProvideCompliantPackageIcon)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsPackable() || context.File.IsTestProject()) return;

        foreach (var icon in context.File
            .Walk()
            .OfType<PackageIcon>()
            .Where(i => i.Value is { Length: > 0 }))
        {
            var info = Resolve(icon.Value!, context.File);

            if (info is null)
            {
                continue;
            }

            if (info.Type != "PNG")
            {
                context.ReportDiagnostic(Descriptor, icon, icon.Value, "is recommended to be a PNG");
            }
            if ((info.Height != 128 || info.Width != 128) && (info.Height != default && info.Width != default))
            {
                context.ReportDiagnostic(Descriptor, icon, icon.Value, $"is recommended to be 128x128 not {info.Width}x{info.Height}");
            }
            if (info.Size > 1_000_000)
            {
                context.ReportDiagnostic(Descriptor, icon, icon.Value, $"must be less then 1MB");
            }
        }
    }

    private static ImageInfo? Resolve(string iconValue, MsBuildProject project)
    {
        var file = project.Path.Directory.File(iconValue);

        if (!file.Exists && project
        .Walk()
            .OfType<BuildAction>()
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
