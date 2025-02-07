using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.DefineGlobalPackageReferenceInDirectoryPackagesOnly"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineGlobalPackageReferenceInDirectoryPackagesOnly()
    : MsBuildProjectFileAnalyzer(Rule.DefineGlobalPackageReferenceInDirectoryPackagesOnly)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.AllExceptDirectoryPackages;

    /// <inheritdoc/>
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach (var reference in context.File.ItemGroups.SelectMany(g => g.GlobalPackageReferences))
        {
            context.ReportDiagnostic(Descriptor, reference);
        }
    }
}
