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

## Parsing INI files
|     File | Mean       | Gen0     | Gen1     | Gen2     | Allocated  |      Speed |
|---------:|-----------:|---------:|---------:|---------:|-----------:|-----------:|
|   27 LoC |   7.611 us |   1.6861 |   0.0610 |        - |   20.70 KB | 282 ns/LoC |
|   36 LoC |   7.957 us |   1.5411 |   0.0458 |        - |   18.91 KB | 221 ns/LoC |
| 1220 LoC | 754.667 us | 272.4609 | 272.4609 | 272.4609 | 2168.57 KB | 619 ns/Loc |

## Parsing Globs
The purpose was never speed, 
| Method               | Expression           | Mean      |
|--------------------- |--------------------- |----------:|
| DotNetProjectFile_IO | [Dd]ebug             |  81.95 ns |
| GlobExpressions_Glob | [Dd]ebug             | 215.51 ns |
| DotNet_Globbing      | [Dd]ebug             | 189.20 ns |
| DotNetProjectFile_IO | *.{cs,vb,ts}         | 242.29 ns |
| GlobExpressions_Glob | *.{cs,vb,ts}         | 311.40 ns |
| DotNet_Globbing      | *.{cs,vb,ts}         | 236.58 ns |
| DotNetProjectFile_IO | *multiple xml files* | 339.46 ns |
| GlobExpressions_Glob | *multiple xml files* | 362.53 ns |
| DotNet_Globbing      | *multiple xml files* | 385.11 ns |
| DotNetProjectFile_IO | *.*                  |  69.31 ns |
| GlobExpressions_Glob | *.*                  | 134.49 ns |
| DotNet_Globbing      | *.*                  | 207.82 ns |

## Parsing .gitignore files
|    File | Mean     | Gen0    | Gen1   | Allocated |      Speed |
|--------:|---------:|--------:|-------:|----------:|-----------:|
| 344 LoC | 112.0 us |  9.2773 | 2.1973 | 114.00 KB | 326 ns/LoC |
| 606 LoC | 222.7 us | 13.4277 | 4.3945 | 166.49 KB | 367 ns/LoC |

## Licensing
| PrepareText            | Mean     |
|----------------------- |---------:|
| new StringBuilder()    | 1.495 us |
| new StringBuilder(len) | 1.364 us |
| string.Concat()        | 6.749 us |
| new char[len]          | 5.881 us |

| Method                       | N | Mean       | Error    | StdDev   | Gen0   | Allocated |
|----------------------------- |-- |-----------:|---------:|---------:|-------:|----------:|
| SHA1_Static                  | 1 | 2,152.2 ns | 15.57 ns | 13.01 ns | 0.3090 |   1.27 KB |
| SHA1_Object                  | 1 | 1,996.1 ns | 14.38 ns | 13.45 ns | 0.3204 |   1.31 KB |
| SHA1_Managed                 | 1 | 1,996.4 ns | 13.02 ns | 11.54 ns | 0.3204 |   1.31 KB |
| SHA1_Incremental             | 1 | 1,956.1 ns |  9.35 ns |  8.74 ns | 0.3090 |   1.27 KB |
| SHA256_Static                | 1 | 1,055.4 ns | 19.98 ns | 23.00 ns | 0.3185 |   1.30 KB |
| SHA256_Object                | 1 |   926.7 ns | 18.56 ns | 19.06 ns | 0.3319 |   1.36 KB |
| SHA256_Managed               | 1 |   929.9 ns | 18.31 ns | 17.98 ns | 0.3319 |   1.36 KB |
| SHA256_Incremental           | 1 |   868.8 ns |  7.77 ns |  6.88 ns | 0.3185 |   1.30 KB |
| SHA256_Incremental_Truncated | 1 |   874.0 ns | 16.87 ns | 15.78 ns | 0.3185 |   1.30 KB |
| SHA512_Incremental_Truncated | 1 | 1,140.4 ns | 15.56 ns | 12.99 ns | 0.3262 |   1.34 KB |
