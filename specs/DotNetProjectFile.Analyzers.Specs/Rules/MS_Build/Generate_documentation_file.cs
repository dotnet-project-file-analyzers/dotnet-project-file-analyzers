namespace Rules.MS_Build.Generate_documentation_file;

public class Reports
{
    [Test]
    public void on_missing_property() => new EnableGenerateDocumentationFile()
        .ForProject("GenerateDocumentationFileMissing.cs")
        .HasIssue(new Issue("Proj0244", "Define the <GenerateDocumentationFile> node with value 'true' or define the <DocumentationFile> node with a valid file path or define the <IsPackable> node with value 'false'.")
        .WithSpan(00, 00, 07, 10));


    [Test]
    public void on_disabled_property() => new EnableGenerateDocumentationFile()
        .ForProject("GenerateDocumentationFileDisabledWithoutFile.cs")
        .HasIssue(new Issue("Proj0244", "Define the <GenerateDocumentationFile> node with value 'true' or define the <DocumentationFile> node with a valid file path or define the <IsPackable> node with value 'false'.")
        .WithSpan(05, 04, 05, 64));

    [Test]
    public void on_disabled_property_with_file_path() => new EnableGenerateDocumentationFile()
        .ForProject("GenerateDocumentationFileDisabledWithFile.cs")
        .HasIssue(new Issue("Proj0244", "Define the <GenerateDocumentationFile> node with value 'true' or define the <DocumentationFile> node with a valid file path or define the <IsPackable> node with value 'false'.")
        .WithSpan(05, 04, 05, 64));

    [Test]
    public void on_missing_property_with_empty_file_path() => new EnableGenerateDocumentationFile()
    .ForProject("GenerateDocumentationFileMissingWithEmptyFile.cs")
    .HasIssue(new Issue("Proj0244", "Define the <GenerateDocumentationFile> node with value 'true' or define the <DocumentationFile> node with a valid file path or define the <IsPackable> node with value 'false'.")
    .WithSpan(05, 04, 05, 43));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("GenerateDocumentationFileEnabled.cs")]
    [TestCase("GenerateDocumentationFileEnabledWithFile.cs")]
    [TestCase("GenerateDocumentationFileMissingWithFile.cs")]
    public void Projects_without_issues(string project) => new EnableGenerateDocumentationFile()
        .ForProject(project)
        .HasNoIssues();
}
