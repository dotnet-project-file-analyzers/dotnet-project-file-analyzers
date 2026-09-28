namespace Rules.MS_Build.Remove_legacy_nodes;

public class Reports
{
    [Test]
    public void legacy_nodes() => new RemoveLegacyNodes().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <AppDesignerFolder>Properties</AppDesignerFolder>
            <AssemblyCompany>Acme</AssemblyCompany>
            <AssemblyConfiguration>Debug</AssemblyConfiguration>
            <AssemblyCopyright>Copyright Acme</AssemblyCopyright>
            <AssemblyDescription>Description</AssemblyDescription>
            <AssemblyFileVersion>1.0.0.0</AssemblyFileVersion>
            <AssemblyInformationalVersion>1.0.0</AssemblyInformationalVersion>
            <AssemblyProduct>Product</AssemblyProduct>
            <AssemblyTitle>Title</AssemblyTitle>
            <AssemblyVersion>1.0.0.0</AssemblyVersion>
            <FileAlignment>512</FileAlignment>
            <IISExpressSSLPort>44368</IISExpressSSLPort>
            <Install>true</Install>
            <InstallFrom>Web</InstallFrom>
            <NuGetPackageImportStamp>2b1c1a1e-0000-0000-0000-000000000000</NuGetPackageImportStamp>
            <ProjectGuid>{ECC1C67A-4BD7-400F-92C5-39395F05B586}</ProjectGuid>
            <ProjectTypeGuids>{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
            <SchemaVersion>2.0</SchemaVersion>
            <SlnVersion>12.00</SlnVersion>
            <StartAction>Program</StartAction>
            <UseGlobalApplicationHostFile>true</UseGlobalApplicationHostFile>
            <UseIISExpress>true</UseIISExpress>
            <UseVSToolPath>true</UseVSToolPath>
          </PropertyGroup>

          <ProjectExtensions />

          <ItemGroup>
            <BootstrapperPackage Include="Microsoft.Net.Framework.3.5" />
          </ItemGroup>

          <ItemGroup>
            <Reference Include="Example">
              <HintPath>..\packages\Example.1.0.0\lib\net45\Example.dll</HintPath>
            </Reference>
          </ItemGroup>

          <Target Name="Legacy">
            <PropertyGroup>
              <TargetFrameworkProfile>Client</TargetFrameworkProfile>
              <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
              <TargetPlatformIdentifier>UAP</TargetPlatformIdentifier>
              <TargetPlatformMinVersion>10.0.14393.0</TargetPlatformMinVersion>
              <TargetPlatformVersion>10.0.16299.0</TargetPlatformVersion>
            </PropertyGroup>
          </Target>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0058", "Remove the legacy AppDesignerFolder node"/*...............*/).WithSpan(04, 04, 04, 53),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyCompany node"/*.................*/).WithSpan(05, 04, 05, 43),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyConfiguration node"/*...........*/).WithSpan(06, 04, 06, 56),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyCopyright node"/*...............*/).WithSpan(07, 04, 07, 57),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyDescription node"/*.............*/).WithSpan(08, 04, 08, 58),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyFileVersion node"/*.............*/).WithSpan(09, 04, 09, 54),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyInformationalVersion node"/*....*/).WithSpan(10, 04, 10, 70),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyProduct node"/*.................*/).WithSpan(11, 04, 11, 46),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyTitle node"/*...................*/).WithSpan(12, 04, 12, 40),
            Issue.WRN("Proj0058", "Remove the legacy AssemblyVersion node"/*.................*/).WithSpan(13, 04, 13, 46),
            Issue.WRN("Proj0058", "Remove the legacy FileAlignment node"/*...................*/).WithSpan(14, 04, 14, 38),
            Issue.WRN("Proj0058", "Remove the legacy IISExpressSSLPort node"/*...............*/).WithSpan(15, 04, 15, 48),
            Issue.WRN("Proj0058", "Remove the legacy Install node"/*.........................*/).WithSpan(16, 04, 16, 27),
            Issue.WRN("Proj0058", "Remove the legacy InstallFrom node"/*.....................*/).WithSpan(17, 04, 17, 34),
            Issue.WRN("Proj0058", "Remove the legacy NuGetPackageImportStamp node"/*.........*/).WithSpan(18, 04, 18, 91),
            Issue.WRN("Proj0058", "Remove the legacy ProjectGuid node"/*.....................*/).WithSpan(19, 04, 19, 69),
            Issue.WRN("Proj0058", "Remove the legacy ProjectTypeGuids node"/*................*/).WithSpan(20, 04, 20, 79),
            Issue.WRN("Proj0058", "Remove the legacy SchemaVersion node"/*...................*/).WithSpan(21, 04, 21, 38),
            Issue.WRN("Proj0058", "Remove the legacy SlnVersion node"/*......................*/).WithSpan(22, 04, 22, 34),
            Issue.WRN("Proj0058", "Remove the legacy StartAction node"/*.....................*/).WithSpan(23, 04, 23, 38),
            Issue.WRN("Proj0058", "Remove the legacy UseGlobalApplicationHostFile node"/*....*/).WithSpan(24, 04, 24, 69),
            Issue.WRN("Proj0058", "Remove the legacy UseIISExpress node"/*...................*/).WithSpan(25, 04, 25, 39),
            Issue.WRN("Proj0058", "Remove the legacy UseVSToolPath node"/*...................*/).WithSpan(26, 04, 26, 39),
            Issue.WRN("Proj0058", "Remove the legacy ProjectExtensions node"/*...............*/).WithSpan(29, 02, 29, 23),
            Issue.WRN("Proj0058", "Remove the legacy BootstrapperPackage node"/*.............*/).WithSpan(32, 04, 32, 65),
            Issue.WRN("Proj0058", "Remove the legacy HintPath node"/*........................*/).WithSpan(37, 06, 37, 74),
            Issue.WRN("Proj0058", "Remove the legacy TargetFrameworkProfile node"/*..........*/).WithSpan(43, 06, 43, 61),
            Issue.WRN("Proj0058", "Remove the legacy TargetFrameworkVersion node"/*..........*/).WithSpan(44, 06, 44, 59),
            Issue.WRN("Proj0058", "Remove the legacy TargetPlatformIdentifier node"/*........*/).WithSpan(45, 06, 45, 62),
            Issue.WRN("Proj0058", "Remove the legacy TargetPlatformMinVersion node"/*........*/).WithSpan(46, 06, 46, 71),
            Issue.WRN("Proj0058", "Remove the legacy TargetPlatformVersion node"/*...........*/).WithSpan(47, 06, 47, 65));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_without_issues(string project) => new RemoveLegacyNodes()
        .ForProject(project)
        .HasNoIssues();
}
