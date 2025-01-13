namespace Rules.MS_Build.Dont_mix_version_and_version_prefix_or_version_suffix;

public class Reports
{
    [Test]
    public void on_version_and_prefix()
       => new DontMixVersionAndVersionPrefixOrVersionSuffix()
        .ForProject("VersionPrefixAndVersion.cs")
        .HasIssue(
           Issue.WRN("Proj0245", "Remove the <Version> node or remove the <VersionPrefix> node.")
           .WithSpan(09, 04, 09, 28));

    [Test]
    public void on_version_and_suffix()
       => new DontMixVersionAndVersionPrefixOrVersionSuffix()
        .ForProject("VersionSuffixAndVersion.cs")
        .HasIssue(
           Issue.WRN("Proj0245", "Remove the <Version> node or remove the <VersionSuffix> node.")
           .WithSpan(09, 04, 09, 28));

    [Test]
    public void on_version_and_prefix_and_suffix()
       => new DontMixVersionAndVersionPrefixOrVersionSuffix()
        .ForProject("VersionPrefixAndSuffixAndVersion.cs")
        .HasIssue(
           Issue.WRN("Proj0245", "Remove the <Version> node or remove the <VersionPrefix> and <VersionSuffix> nodes.")
           .WithSpan(09, 04, 09, 28));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("VersionPrefix.cs")]
    [TestCase("VersionPrefixAndSuffix.cs")]
    [TestCase("VersionSuffixWithoutPrefix.cs")]
    public void Projects_without_issues(string project)
         => new DontMixVersionAndVersionPrefixOrVersionSuffix()
        .ForProject(project)
        .HasNoIssues();
}
