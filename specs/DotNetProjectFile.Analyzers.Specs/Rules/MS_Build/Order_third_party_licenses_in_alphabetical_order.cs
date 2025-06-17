namespace Rules.MS_Build.Order_third_party_licenses_in_alphabetical_order;

public class Reports
{
    [Test]
    public void on_not_alphabetical_order()
       => new OrderThirdPartyLicensesAlphabetically()
            .ForInlineCsproj(@"
            <Project Sdk=""Microsoft.NET.Sdk"">

                <PropertyGroup>
                    <TargetFramework>net8.0</TargetFramework>
                </PropertyGroup>

                <ItemGroup>
                    <ThirdPartyLicense Include=""SonarAnalyzer.CSharp"" Hash=""ZOAgZmx18wSWq5KpOpWd2bB9123"" />
                    <ThirdPartyLicense Include=""SixLabors.ImageSharp"" Hash=""C3au3cYr2n3QFmhQ3SSmTQ"" />
                </ItemGroup>

            </Project>")
            .HasIssue(
                Issue.WRN("Proj0508", "Third-party license 'SixLabors.ImageSharp' is not ordered alphabetically and should appear before 'SonarAnalyzer.CSharp'").WithSpan(8, 20, 8, 102));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new OrderThirdPartyLicensesAlphabetically()
            .ForProject(project)
            .HasNoIssues();
}
