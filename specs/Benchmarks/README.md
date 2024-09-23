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
