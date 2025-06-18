using System.Collections.Frozen;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.NameCoventionBasedMsBuildFilesShouldHaveCorectCasing"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class NameCoventionBasedMsBuildFilesShouldHaveCorectCasing()
    : MsBuildProjectFileAnalyzer(Rule.NameCoventionBasedMsBuildFilesShouldHaveCorectCasing)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.SDK;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => true;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var file in Files(context.File.Path.Directory))
        {
            if (SpecialFiles.TryGetValue(file.Name, out var name) && file.Name != name)
            {
                var location = Location.Create(file.ToString(), new(0, 1), new(new(0, 0), new(0, 1)));
                context.ReportDiagnostic(Descriptor, location, file.Name, name);
            }
        }
    }

    private static IEnumerable<IOFile> Files(IODirectory directory)
    {
        if (directory.Files() is { } files)
        {
            foreach (var file in files)
            {
                yield return file;
            }
        }

        if (directory.SubDirectories() is { } dirs)
        {
            foreach (var dir in dirs)
            {
                foreach (var file in Files(dir))
                {
                    yield return file;
                }
            }
        }
    }

    private static readonly FrozenDictionary<string, string> SpecialFiles = Init().ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

    private static IEnumerable<KeyValuePair<string, string>> Init() =>
    [
        File(".editorconfig"),
        File(".globalconfig"),
        File(".net.csproj"),
        File("CompatibilitySuppressions.xml"),
        File("Directory.Build.props"),
        File("Directory.Build.targets"),
        File("Directory.Packages.props"),
        File("global.json"),
        File("NuGet.config"),
        File("packages.lock.json"),
    ];

    private static KeyValuePair<string, string> File(string name) => new(name, name);
}
