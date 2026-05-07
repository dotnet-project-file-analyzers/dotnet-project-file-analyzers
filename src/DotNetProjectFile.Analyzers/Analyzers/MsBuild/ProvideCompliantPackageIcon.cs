namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProvideCompliantPackageIcon() : MsBuildProjectFileAnalyzer(Rule.ProvideCompliantPackageIcon)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsPackable() || context.File.IsTestProject()) return;

        foreach (var icon in context.File
            .Walk()
            .OfType<PackageIcon>()
            .Where(i => i.Value is { Length: > 0 }))
        {
            var info = ResolveImage(icon, icon.Value!, context);

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

    private static ImageInfo? ResolveImage(Node iconNode, string iconValue, ProjectFileAnalysisContext context)
    {
        var (root, literal) = context.Resolve(iconNode, iconValue);
        var file = root.File(literal);

        if (!file.Exists && context.File
            .Walk()
            .OfType<BuildAction>()
            .SelectMany(a => a.IncludeAndUpdate)
            .FirstOrDefault(a => a.EndsWith(iconValue)) is { } action)
        {
            file = context.File.Path.Directory.File(action);
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
