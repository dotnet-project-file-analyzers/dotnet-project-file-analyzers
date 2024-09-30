# Benchmarks
To monitor performance.

## Get Diagnostics
Comparison of the build times, with and without running the .NET project file
analyzers.

| Project        |     Build |    Build+ | Analysis |
|----------------|----------:|----------:|---------:|
| CompliantCS    |  0.267 ms |  7.859 ms | 7.592 ms |
| CompliantVB    |  0.440 ms |  7.806 ms | 7.366 ms |
| .NET analyzers | 73.621 ms | 73.621 ms | 0.586 ms |

# Parsing INI files
|     File |        Mean |       per LoC |
|---------:|------------:|--------------:|
|   27 LoC |    49.71 us |   1.84 us/LoC |
|   36 LoC |    54.55 us |   1.52 us/LoC |
| 1220 LoC | 7,819.40 us |   6.41 us/LoC |

# Parsing Globs
The purpose was never speed, 
| Method               | Expression           | Mean      |
|--------------------- |--------------------- |----------:|
| DotNetProjectFile_IO | [Dd]ebug             |  89.65 ns |
| GlobExpressions_Glob | [Dd]ebug             | 212.86 ns |
| DotNet_Globbing      | [Dd]ebug             | 180.70 ns |
| DotNetProjectFile_IO | *.{cs,vb,ts}         | 251.92 ns |
| GlobExpressions_Glob | *.{cs,vb,ts}         | 305.30 ns |
| DotNet_Globbing      | *.{cs,vb,ts}         | 248.20 ns |
| DotNetProjectFile_IO | *multiple xml files* | 338.47 ns |
| GlobExpressions_Glob | *multiple xml files* | 370.06 ns |
| DotNet_Globbing      | *multiple xml files* | 373.25 ns |
| DotNetProjectFile_IO | *.*                  |  73.34 ns |
| GlobExpressions_Glob | *.*                  | 130.10 ns |
| DotNet_Globbing      | *.*                  | 209.99 ns |
