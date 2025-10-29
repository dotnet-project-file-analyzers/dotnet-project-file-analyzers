namespace Rules.MS_Build.Define_output_type;

public class Reports
{
    [Test]
    public void on_no_output_type()
       => new DefineOutputType()
       .ForProject("NoOutputType.cs")
       .HasIssue(
           Issue.WRN("Proj0010", "Define the <OutputType> node explicitly"));

    [TestCase(nameof(Language.CSharp))]
    [TestCase(nameof(Language.FSharp))]
    [TestCase(nameof(Language.VisualBasic))]
    public void on_no_output_type_for_various_languages(Language language)
        => new DefineOutputType()
        .ForInlineProject(language, """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
              </PropertyGroup>
            </Project>
            """)
        .HasIssue(
           Issue.WRN("Proj0010", "Define the <OutputType> node explicitly")
            .WithSpan(0, 0, 0, 32));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new DefineOutputType()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void on_output_type_for_fsharp()
        => new DefineOutputType()
        .ForInlineFsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                    <OutputType>Exe</OutputType>
                </PropertyGroup>
            </Project>
            """)
        .HasNoIssues();
}
