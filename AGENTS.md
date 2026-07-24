# AGENTS.md

## Build & Test Commands

```bash
# Build
dotnet build dotnet-project-file-analyzers.slnx -c Release

# Test (must filter out Generators category — CI does this too)
dotnet test dotnet-project-file-analyzers.slnx -c Release --no-build --filter TestCategory!=Generators

# Pack (version passed via -p:PackageVersion=)
dotnet pack src/DotNetProjectFile.Analyzers/DotNetProjectFile.Analyzers.csproj -c Release -p:PackageVersion=1.15.2
```

No separate lint/typecheck step — `EnforceCodeStyleInBuild=true` in `props/common.props` means code style rules run during `dotnet build`. Fix build warnings to satisfy style.

## SDK & Toolchain

- **.NET SDK 10.0.300** pinned in `global.json` (`rollForward: latestPatch`).
- **Solution format**: `dotnet-project-file-analyzers.slnx` (new XML-based SLNX, not legacy .sln).
- **C# 14.0**, nullable enabled, `netstandard2.0` for the analyzer itself, `net10.0` for tests.

## Architecture

This is a **Roslyn analyzer package** (160+ diagnostics) for .NET project files (.csproj, .vbproj, .resx, NuGet.config, .editorconfig, .slnx, INI).

| Path | Purpose |
|---|---|
| `.net.csproj` | SDK | Allows `DotNetProjectFile.Analyzers` to run Roslyn Analyzers on MSBuild files. Ignore it. |
| `src/DotNetProjectFile.Analyzers/` | Main analyzer library (`netstandard2.0`) |
| `src/.../Analyzers/` | Analyzer implementations (MsBuild, Resx, Ini, NuGetConfig, Slnx, Generic) |
| `src/.../Rule.cs` (+ partials) | All `DiagnosticDescriptor` definitions |
| `src/.../build/` | MSBuild `.props`/`.targets` shipped in the NuGet package |
| `specs/Specs/` | Unit tests (NUnit) |
| `specs/TestData/` | Shared test data (embedded files) |
| `specs/Bench/` | Benchmarks (BenchmarkDotNet) |
| `projects/` | 120+ fixture projects used as test input data |
| `props/common.props` | Shared MSBuild properties for all projects |

Rule numbering: `Proj00xx` general, `Proj02xx` packaging, `Proj03xx` NuGet config, `Proj05xx` licensing, `Proj08xx` CPM, `Proj17xx` formatting, `Proj2xxx` RESX, `Proj3xxx` generic, `Proj4xxx` INI/EditorConfig, `Proj5xxx` SLNX.

## Testing Quirks

- Tests use **NUnit 4** + **Verify** (snapshot testing) + **CodeAnalysis.TestTools**.
- Verify snapshots: `.verified.txt` is the baseline; `.received.txt` is generated on failure. Accept by replacing verified with received.
- `specs/TestData/` is a separate project embedding files from `specs/TestData/Files/`.
- Fixture projects in `projects/` are linked into the Specs project as `<None>` items.
- Windows-specific tests use `DefineConstants: Is_Windows` conditional compilation.
- CI always runs with `--filter TestCategory!=Generators`.

## Code Style

- `.editorconfig`: 4-space indent for `.cs`; 2-space indent for `.csproj`/`.props`/`.targets`/`.slnx`.
- Expression-bodied members preferred for methods, properties, constructors, accessors, indexers, operators.
- Multiple Roslyn analyzer packages enforce style: StyleCop, SonarAnalyzer, Qowaiv, AsyncFixer.
- `RestoreLockedMode` enforced in CI — after changing dependencies, run `dotnet restore --force-evaluate` to update `packages.lock.json`.

## CI

- **Build workflow** (`build-publish.yml`): build → test (with `TestCategory!=Generators` filter) → pack on tag → publish to NuGet.org.
- Dependabot: weekly NuGet + GitHub Actions updates.
- Spell/grammar checks on `docs/**` via LanguageTool and Misspell reviewdog workflows.

## Adding a New Diagnostic

1. Add a `DiagnosticDescriptor` property in `Rule.cs` (or the relevant partial: `Rule.Ini.cs`, `Rule.NuGet.cs`, `Rule.RESX.cs`, `Rule.SLNX.cs`).
2. Implement the analyzer in the appropriate `Analyzers/` subdirectory.
3. Add test fixture projects under `projects/` if needed.
4. Add tests under `specs/Specs/` matching the source structure.
5. Update `docs/` with the new rule documentation.

## Gotchas

* **Do not modify `.net.csproj`:** This file is exclusively for running Roslyn Analyzers. Agents must completely ignore and skip this file during development.
