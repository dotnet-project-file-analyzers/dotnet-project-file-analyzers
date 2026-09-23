---
permalink: /sonar-integration
nav_order: 5
---

# SonarQube Integration
> **Note:** This guide is verified for [SonarQube Cloud](https://www.sonarsource.com/products/sonarqube/cloud/).
  The approach may apply to other [SonarSource](https://www.sonarsource.com)
  products, but this has not been verified.

SonarQube Cloud scans your code for issues and provides an online dashboard for
tracking them. It integrates with DevOps platforms like [GitHub](https://github.com/)
and [Azure DevOps](https://dev.azure.com/), enabling automatic pull request
comments for newly introduced warnings — helping teams catch and address
quality issues early.

## The Challenge
SonarQube's analyzer runs during builds (e.g. as part of a PR check), but by
default it does not pick up warnings reported on files SonarQube does not have
analyzers for, such as `.csproj` and `.props` files. As a result:

- Warnings from this analyzer will **not** appear in SonarQube reports.
- They will **not** generate pull request comments.
- They risk going unnoticed and unresolved.

## Solution: Register `<AdditionalFiles>` as files to analyze
SonarScanner for .NET collects the files it analyzes from the item types listed
in `$(SQAnalysisFileItemTypes)`. That list contains `<Compile>`, `<Content>`,
`<EmbeddedResource>`, and `<None>`, but not `<AdditionalFiles>` — which is
exactly where this package registers the files it reports issues on.

To close that gap, the .NET Project File Analyzers package adds
`AdditionalFiles` to `$(SQAdditionalAnalysisFileItemTypes)`, the extension point
of the scanner for that list:

``` XML
<PropertyGroup>
  <SQAdditionalAnalysisFileItemTypes>$(SQAdditionalAnalysisFileItemTypes);AdditionalFiles</SQAdditionalAnalysisFileItemTypes>
</PropertyGroup>
```

As a result, SonarQube reports the issues found on files such as `.csproj`,
`.props`, and `.targets`, without those files taking part in the build itself.

This behaviour can be disabled by setting `<SonarQubeIntegration>` to `false`.
