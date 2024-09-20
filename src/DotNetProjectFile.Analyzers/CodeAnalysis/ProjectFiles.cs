﻿using DotNetProjectFile.Ini;
using DotNetProjectFile.Resx;

namespace DotNetProjectFile.CodeAnalysis;

public sealed partial class ProjectFiles
{
    public static readonly ProjectFiles Global = new();

    private readonly FileCache<IniFileSyntax> IniFiles = new();
    private readonly FileCache<MsBuildProject> MsBuildProjects = new();
    private readonly FileCache<Resource> ResourceFiles = new();

    public IniFileSyntax? IniFile(IOFile file)
        => IniFiles.TryGet(file, Create_IniFile);

    public MsBuildProject? MsBuildProject(IOFile file)
        => MsBuildProjects.TryGet(file, Create_MsBuildProject);

    public Resource? ResourceFile(IOFile file)
        => ResourceFiles.TryGet(file, Create_ResourceFile);

    public MsBuildProject? UpdateMsBuildProject(CompilationAnalysisContext context)
    {
        var extenion = context.Compilation.Language switch
        {
            LanguageNames.CSharp => ".csproj",
            LanguageNames.VisualBasic => ".vbproj",
            _ => null,
        };

        if (extenion is null || context.Compilation.AssemblyName is not { Length: > 0 } name) return null;

        var file = name + extenion;

        return context.Compilation?.Assembly?.Locations
            .Select(l => IOFile.Parse(l.SourceTree?.FilePath))
            .Where(file => file.HasValue)
            .SelectMany(file => file.Directory.AncestorsAndSelf())
            .Distinct()
            .Select(dir => MsBuildProject(dir.File(file)))
            .OfType<MsBuildProject>()
            .FirstOrDefault();
    }

    public MsBuildProject? UpdateMsBuildProject(AdditionalFileAnalysisContext context)
    {
        var file = IOFile.Parse(context.AdditionalFile.Path);
        return Is.MsBuild(file)
            ? MsBuildProjects.TryGet(file, _ => MsBuild.Project.Load(context.AdditionalFile, this))
            : null;
    }

    public Resource? UpdateResourceFile(AdditionalFileAnalysisContext context)
    {
        var file = IOFile.Parse(context.AdditionalFile.Path);
        return Is.Resource(file)
            ? ResourceFiles.TryGet(file, _ => Resource.Load(context.AdditionalFile, this))
            : null;
    }

    private MsBuildProject Create_MsBuildProject(IOFile file)
        => MsBuild.Project.Load(file, this);

    private static IniFileSyntax Create_IniFile(IOFile file)
        => IniFileSyntax.Parse(Syntax.SyntaxTree.Load(file.OpenRead()));

    private Resource Create_ResourceFile(IOFile file)
        => Resource.Load(file, this);

    private static class Is
    {
        public static bool Ini(IOFile file)
              => file.Extension.IsMatch(".ini")
              || file.Extension.IsMatch(".editorconfig");

        public static bool MsBuild(IOFile file)
            => file.Extension.IsMatch(".csproj")
            || file.Extension.IsMatch(".props")
            || file.Extension.IsMatch(".vbproj");

        public static bool Resource(IOFile file) => file.Extension.IsMatch(".resx");
    }
}
