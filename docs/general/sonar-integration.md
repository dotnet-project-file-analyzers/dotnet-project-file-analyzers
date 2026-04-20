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

## Workaround: Register files as `<Content>`
What does work, is registering all files that are analyzed as `<Content>` for
MSBuild. By doing so, SonarQube will report analysis on these files. To make
life easier, the .NET Project File Analyzers package does this automatically for:
- `**/*.??proj`
- `**/*.config`
- `**/*.props`
- `**/*.resx`
- `**/*.slnx`

If adding these files as content leads to issues, this behaviour can be
disabled by setting `<SonarQubeIntegration>` to `false`.
