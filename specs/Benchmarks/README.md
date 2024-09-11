# Benchmarks
To monitor performance.

## Get Diagnostics
| File        | Mean         | Error       | StdDev       |
|------------ |-------------:|------------:|-------------:|
| CompliantCS |  31,105.5 us |   592.93 us |    974.20 us |
| CompliantVB |     453.7 us |     6.27 us |      5.23 us |
| Project     | 350,334.4 us | 7,834.75 us | 22,854.33 us |


|--------------- |------------ |-------------:|------------:|-------------:|
| GetDiagnostics | CompliantCS |  29,433.9 us |   256.62 us |    227.49 us |
| GetDiagnostics | CompliantVB |     469.1 us |     9.27 us |     13.87 us |
| GetDiagnostics | Project     | 324,856.2 us | 6,036.65 us | 14,110.48 us |
