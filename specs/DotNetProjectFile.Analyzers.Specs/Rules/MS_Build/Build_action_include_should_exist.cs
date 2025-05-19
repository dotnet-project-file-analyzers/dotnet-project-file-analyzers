namespace Rules.MS_Build.Build_action_include_should_exist;

public class Reports
{
    [Test]
    public void empty_includes()
        => new BuildActionIncludeShouldExist()
        .ForProject("EmptyInclude.cs")
        .HasIssues(
            Issue.WRN("Proj0022", "The Include '*.txt' of <None> does not match any files" /*..........*/).WithSpan(08, 04, 08, 28),
            Issue.WRN("Proj0022", "The Include 'DoesNotExist.cs' of <None> does not exist" /*..........*/).WithSpan(09, 04, 09, 38),
            Issue.WRN("Proj0022", "The Include '*.vbproj' of <Content> does not match any files" /*....*/).WithSpan(10, 04, 10, 43),
            Issue.WRN("Proj0022", "The Include '**/*.bin' of <EmbeddedResource> does not match any files").WithSpan(11, 04, 11, 43),
            Issue.WRN("Proj0022", "The Include '*.unknown' of <AdditionalFiles> does not match any files").WithSpan(12, 04, 12, 43)
        );

    [Test]
    public void empty_includes_config_files()
        => new BuildActionIncludeShouldExist()
        .ForProject("ConfigFilesExistence.cs")
        .HasIssues(
            Issue.WRN("Proj0022", "The Include 'missing.editor' of <EditorConfgFiles> does not exist" /*....*/).WithSpan(12, 04, 12, 49),
            Issue.WRN("Proj0022", "The Include 'missing.config' of <GlobalAnalyzerConfigFiles> does not exist").WithSpan(13, 04, 13, 58)
        );
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_with_analyzers(string project)
         => new BuildActionIncludeShouldExist()
        .ForProject(project)
        .HasNoIssues();
}
