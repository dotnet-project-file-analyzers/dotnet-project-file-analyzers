---
nav_order: 4
---

# NuGet packages
Microsoft provides [best practices](https://learn.microsoft.com/nuget/create-packages/package-authoring-best-practices)
to give NuGet package authors a reference to create and publish high-quality
packages.

.NET project file analyzers provides rules for most of them:

| Rule                             | Recommendation
|:--------------------------------:|----------------------------------------------
| [Proj0200](../rules/Proj0200.md) | Explicitly define the package as packable.
| [Proj0201](../rules/Proj0201.md) | Consider using SemVer to version your NuGet package.
| [Proj0202](../rules/Proj0202.md) | Do include a short description (up to 4000 characters) to describe your package.
| [Proj0203](../rules/Proj0203.md) | Do use the author field for your or your organization's "pretty name."
| [Proj0204](../rules/Proj0204.md) | Do include several tags with key terms related to your package to enhance discoverability.
| [Proj0205](../rules/Proj0205.md) | Do include a link to an associated repository
| [Proj0206](../rules/Proj0206.md) | Do include a link to an associated project or company website.
| [Proj0207](../rules/Proj0207.md) | Do add a copyright notice to your package with "Copyright (c) <name/company> <year>."
| [Proj0208](../rules/Proj0208.md) | Do include release notes with each update describing what changes were made.
| [Proj0209](../rules/Proj0209.md) | Do add a README markdown file that provides an overview of what your package does and how to get started.
| [Proj0210](../rules/Proj0210.md) | Do include a license expression or license file in your package.
| [Proj0211](../rules/Proj0211.md) | Do not use the deprecated PackageLicenseUrl to include a license.
| [Proj0212](../rules/Proj0212.md) | Consider including an icon with your package to help to visually differentiate it. It's a relatively small addition that can improve perception of package quality.
| [Proj0213](../rules/Proj0213.md) | Consider adding the icon url for maximum compatibility.
| [Proj0214](../rules/Proj0214.md) | Do include the package id.
| [Proj0216](../rules/Proj0216.md) | define the product name.
| *none*                           | Consider choosing a NuGet package name with a prefix that meets NuGet's [prefix reservation criteria](https://learn.microsoft.com/nuget/nuget-org/id-prefix-reservation).
| [Proj0215](../rules/Proj0215.md) | Do use an image that is 128x128 and has a transparent background (PNG) for the best viewing results.
|                                  | Consider setting up Source Link to automatically add source control metadata to your NuGet package and make your library easier to debug.
|                                  | Do choose an open source license to make your package open source.
| [Proj0243](../rules/Proj0243.md) | Generate a software bill of materials to be compliant with USA legislations.
| [Proj0244](../rules/Proj0244.md) | Include source code documentation for public API.

## Further reading
* [learn.microsoft.com/nuget/reference/msbuild-targets](https://learn.microsoft.com/nuget/reference/msbuild-targets)
