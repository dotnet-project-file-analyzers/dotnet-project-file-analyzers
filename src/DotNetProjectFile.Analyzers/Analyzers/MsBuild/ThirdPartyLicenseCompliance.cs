using DotNetProjectFile.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Validates the <see cref="ThirdPartyLicense"/> node.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ThirdPartyLicenseCompliance() : MsBuildProjectFileAnalyzer(
    Rule.ThirdPartyLicenseRequiresInclude,
    Rule.ThirdPartyLicenseRequiresHash,
    Rule.ThirdPartyLicenseIsUnconditional)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach(var license in context.File.Project.ItemGroups
            .Children<ThirdPartyLicense>(n => n.Project == context.File.Project))
        {
            if (license.Include is not { Length: > 0})
            {
                context.ReportDiagnostic(Rule.ThirdPartyLicenseRequiresInclude, license, "has not been specified");
            }
            else if(Glob.TryParse(license.Include) is null)
            {
                context.ReportDiagnostic(Rule.ThirdPartyLicenseRequiresInclude, license, "is not valid GLOB pattern");
            }

            if(license.Hash is null)
            {
                context.ReportDiagnostic(Rule.ThirdPartyLicenseRequiresHash, license, "has not been specified");
            }
            else if(!IsBase64Hash(license.Hash))
            {
                context.ReportDiagnostic(Rule.ThirdPartyLicenseRequiresHash, license, "is not valid");
            }
            
            if (license.Conditions().Any())
            {
                context.ReportDiagnostic(Rule.ThirdPartyLicenseIsUnconditional, license);
            }
        }
    }

    private static bool IsBase64Hash(string hash)
        => hash.Length == 22
        && hash.All(IsBase64Hash);

    private static bool IsBase64Hash(char c)
        => (c >= 'A' && c <= 'Z')
        || (c >= 'a' && c <= 'z')
        || (c >= '0' && c <= '9')
        || c is '+' or '_';
}
