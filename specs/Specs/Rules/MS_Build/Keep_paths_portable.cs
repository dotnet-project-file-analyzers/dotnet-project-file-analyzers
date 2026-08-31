namespace Rules.MS_Build.Keep_paths_portable;

public class Reports
{
#if Is_Windows
    [Test]
    public void on_absolute_paths() => new KeepPathsPortable().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <PackageIcon>C:\assets\icon.png</PackageIcon>
            <PackageLicenseFile>C:\licenses\license.txt</PackageLicenseFile>
            <PackageOutputPath>C:\nupkg\</PackageOutputPath>
            <PackageReadmeFile>C:\docs\readme.md</PackageReadmeFile>
            <PublishDir>C:\publish\</PublishDir>
          </PropertyGroup>

          <ItemGroup>
            <Folder Include="C:\folders\myfolder\" />
            <AdditionalFiles Include="C:\files\myfile.txt" />
            <None Include="C:\files\none.txt" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0057", "Make 'C:\\assets\\icon.png' a portalbe path"/*......*/).WithSpan(04, 04, 04, 49),
            Issue.WRN("Proj0057", "Make 'C:\\licenses\\license.txt' a portalbe path"/*.*/).WithSpan(05, 04, 05, 68),
            Issue.WRN("Proj0057", "Make 'C:\\nupkg\\' a portalbe path"/*...............*/).WithSpan(06, 04, 06, 52),
            Issue.WRN("Proj0057", "Make 'C:\\docs\\readme.md' a portalbe path"/*.......*/).WithSpan(07, 04, 07, 60),
            Issue.WRN("Proj0057", "Make 'C:\\publish\\' a portalbe path"/*.............*/).WithSpan(08, 04, 08, 40),
            Issue.WRN("Proj0057", "Make 'C:\\folders\\myfolder\\' a portalbe path"/*...*/).WithSpan(12, 04, 12, 45),
            Issue.WRN("Proj0057", "Make 'C:\\files\\myfile.txt' a portalbe path"/*.....*/).WithSpan(13, 04, 13, 53),
            Issue.WRN("Proj0057", "Make 'C:\\files\\none.txt' a portalbe path"/*.......*/).WithSpan(14, 04, 14, 40));
#else
[Test]
    public void on_absolute_paths() => new KeepPathsPortable().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <PackageIcon>/assets/icon.png</PackageIcon>
            <PackageLicenseFile>/licenses/license.txt</PackageLicenseFile>
            <PackageOutputPath>/nupkg</PackageOutputPath>
            <PackageReadmeFile>/docs/readme.md</PackageReadmeFile>
            <PublishDir>/publish</PublishDir>
          </PropertyGroup>

          <ItemGroup>
            <Folder Include="/folders/myfolder" />
            <AdditionalFiles Include="/files/myfile.txt" />
            <None Include="/files/none.txt" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0057", "Make '/assets/icon.png' a portalbe path"/*......*/).WithSpan(04, 04, 04, 47),
            Issue.WRN("Proj0057", "Make '/licenses/license.txt' a portalbe path"/*.*/).WithSpan(05, 04, 05, 66),
            Issue.WRN("Proj0057", "Make '/nupkg' a portalbe path"/*...............*/).WithSpan(06, 04, 06, 49),
            Issue.WRN("Proj0057", "Make '/docs/readme.md' a portalbe path"/*.......*/).WithSpan(07, 04, 07, 58),
            Issue.WRN("Proj0057", "Make '/publish' a portalbe path"/*.............*/).WithSpan(08, 04, 08, 37),
            Issue.WRN("Proj0057", "Make '/folders/myfolder' a portalbe path"/*...*/).WithSpan(12, 04, 12, 42),
            Issue.WRN("Proj0057", "Make '/files/myfile.txt' a portalbe path"/*.....*/).WithSpan(13, 04, 13, 51),
            Issue.WRN("Proj0057", "Make '/files/none.txt' a portalbe path"/*.......*/).WithSpan(14, 04, 14, 38));
#endif
}

public class Guards
{
    [Test]
    public void relative_paths() => new KeepPathsPortable().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <PackageIcon>assets/icon.png</PackageIcon>
            <PackageLicenseFile>licenses/license.txt</PackageLicenseFile>
            <PackageOutputPath>../nupkg</PackageOutputPath>
            <PackageReadmeFile>./docs.readme.md</PackageReadmeFile>
            <PublishDir>publish</PublishDir>
          </PropertyGroup>

          <ItemGroup>
            <Folder Include="myfolder\" />
            <AdditionalFiles Include="files\myfile.txt" />
            <None Include="files\none.txt" />
          </ItemGroup>

        </Project>
        """)
         .HasNoIssues();
}
