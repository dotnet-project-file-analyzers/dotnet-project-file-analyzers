---
permalink: /sonar-integration
nav_order: 5
---

# SonarQube Integration

> **Note:** This guide is verified for [SonarCloud](https://www.sonarsource.com/products/sonarcloud/). The approach may apply to other [SonarSource](https://www.sonarsource.com) products such as SonarQube, but this has not been verified.

SonarCloud scans your code for issues and provides an online dashboard for tracking them. It integrates with DevOps platforms like [GitHub](https://github.com/) and [Azure DevOps](https://dev.azure.com/), enabling automatic pull request comments for newly introduced warnings — helping teams catch and address quality issues early.

---

## The Problem

SonarCloud's analyzer runs during builds (e.g. as part of a PR check), but by default it does not pick up warnings reported on files SonarCloud does not have analyzers for, such as `.??proj` and `.props` files. As a result:

- Warnings from this analyzer will **not** appear in SonarCloud reports.
- They will **not** generate pull request comments.
- They risk going unnoticed and unresolved.

---

## Workaround: Register files as `<Content>`

There is no native solution for this yet. The workaround is to register the relevant file types as `<Content>` items in MSBuild. SonarCloud treats content files as analyzable, so warnings on them will surface in reports and PR annotations.

### Recommended implementation

It is recommended to use a conditioned `ItemGroup` driven by a `SonarCloudIntegration` property. Be aware that `<Content>` might alter the output. `SonarCloudIntegration` should always be enabled, unless this leads to issues such as undesired altered output of the resulting build.

```xml
<!--
  SonarCloud picks up warnings on non-compiled files when they are included
  as <Content>. CopyToOutputDirectory and Visible are both disabled to avoid
  build overhead and Solution Explorer clutter.
-->
<ItemGroup Condition="'$(SonarCloudIntegration)' == 'true'">
  <Content Include="**/*.??proj"  CopyToOutputDirectory="Never" Visible="false" />
  <Content Include="**/*.config"  CopyToOutputDirectory="Never" Visible="false" />
  <Content Include="**/*.props"   CopyToOutputDirectory="Never" Visible="false" />
  <Content Include="**/*.resx"    CopyToOutputDirectory="Never" Visible="false" />
  <Content Include="**/*.slnx"    CopyToOutputDirectory="Never" Visible="false" />
</ItemGroup>
```

Set the `SonarCloudIntegration` property to `true` in whichever build context runs your SonarCloud analysis — typically your CI pipeline or a dedicated MSBuild property in your pipeline configuration.