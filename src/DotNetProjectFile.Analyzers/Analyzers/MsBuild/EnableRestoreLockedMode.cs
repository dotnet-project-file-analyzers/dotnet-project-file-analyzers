using System.Text.RegularExpressions;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class EnableRestoreLockedMode() : MsBuildProjectFileAnalyzer(Rule.EnableRestoreLockedMode)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.File;
        if (!project.PackagesRestoredWithLockFile())
        {
            return;
        }
        else if (project.Property<RestoreLockedMode>() is not { } node)
        {
            context.ReportDiagnostic(Descriptor, context.File);
            return;
        }
        else if (node.Value != true)
        {
            context.ReportDiagnostic(Descriptor, node);
            return;
        }
        else
        {
            var cur = (Node)node;
            while (cur is not null)
            {
                if (IsValidCondition(cur.Condition))
                {
                    return;
                }

                cur = cur.Parent;
            }

            context.ReportDiagnostic(Descriptor, node);
        }
    }

    private static bool IsValidCondition(string? cond)
    {
        if (cond is null)
        {
            return false;
        }

        var normalized = Regex.Replace(cond, @"\s+", string.Empty);
        return normalized == "'$(ContinuousIntegrationBuild)'=='true'"
            || normalized == "'true'=='$(ContinuousIntegrationBuild)'";
    }
}
