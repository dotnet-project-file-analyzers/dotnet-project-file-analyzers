namespace Rules.MS_Build.Enable_nullability;

public class CSharp
{
    public class Reports
    {
        [Test]
        public void on_missing_property() => new EnableNullabilityCSharp().ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
            
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
            
            </Project>
            """)
            .HasIssues(Issue.WRN("Proj0055", "Enable nullability analysis").WithSpan(00, 00, 00, 32));

        [Test]
        public void on_disabled_property() => new EnableNullabilityCSharp().ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
            
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <Nullable>disabled</Nullable>
              </PropertyGroup>
            
            </Project>
            """)
            .HasIssues(Issue.WRN("Proj0055", "Enable nullability analysis").WithSpan(04, 04, 04, 33));

        [Test]
        public void on_warning_only() => new EnableNullabilityCSharp().ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
            
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <Nullable>warnings</Nullable>
              </PropertyGroup>
            
            </Project>
            """)
           .HasIssues(Issue.WRN("Proj0055", "Enable nullability analysis").WithSpan(04, 04, 04, 33));
    }

    public class Guards
    {
        [TestCase("enabled")]
        [TestCase("annotations")]
        public void when_enabled(string kind) => new EnableNullabilityCSharp().ForInlineCsproj($"""
            <Project Sdk="Microsoft.NET.Sdk">
            
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <Nullable>{kind}</Nullable>
              </PropertyGroup>
            
            </Project>
            """)
            .HasNoIssues();
    }
}
public class FSharp
{
    public class Reports
    {
        [Test]
        public void on_missing_property() => new EnableNullabilityFSharp().ForInlineFsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
            
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
            
            </Project>
            """)
            .HasIssues(Issue.WRN("Proj0056", "Enable nullability analysis").WithSpan(00, 00, 00, 32));

        [Test]
        public void on_disabled_property() => new EnableNullabilityFSharp().ForInlineFsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
            
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <Nullable>disabled</Nullable>
              </PropertyGroup>
            
            </Project>
            """)
            .HasIssues(Issue.WRN("Proj0056", "Enable nullability analysis").WithSpan(04, 04, 04, 33));
    }

    public class Guards
    {
        [Test]
        public void when_enabled() => new EnableNullabilityFSharp().ForInlineFsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
            
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <Nullable>enabled</Nullable>
              </PropertyGroup>
            
            </Project>
            """)
            .HasNoIssues();
    }
}
