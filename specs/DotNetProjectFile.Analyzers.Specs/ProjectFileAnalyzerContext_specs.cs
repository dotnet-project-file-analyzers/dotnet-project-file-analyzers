using DotNetProjectFile.Diagnostics;
using DotNetProjectFile.MsBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFileAnalyzerContext_specs;

public class MS_Build_properties
{
    [Test]
    public void can_be_accessed() => new GetProperty()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <CustomProperty>Hello, world!</CustomProperty>
  </PropertyGroup>

</Project>")
        .HasIssue(
            Issue.WRN("AdHoc01", "CustomProperty has value 'Hello, world!'."));
}

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
internal sealed class GetProperty() : MsBuildProjectFileAnalyzer(Specs.TestTools.AdHoc.Rule("CustomProperty has value '{0}'."))
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var x = context.File.Walk().OfType<Unknown>()
            .LastOrDefault(x => x.LocalName == "CustomProperty");

        var value = context.GetMsBuildProperty("CustomProperty");
        context.ReportDiagnostic(Descriptor, context.File, x?.Value);
    }
}
