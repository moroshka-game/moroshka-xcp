using BenchmarkDotNet.Running;
using Moroshka.Xcp.Benchmark;

BenchmarkRunner.Run<Benchmark>();

/*
| Method                           | Mean       | Error    | StdDev   | Rank | Gen0   | Gen1   | Allocated |
|--------------------------------- |-----------:|---------:|---------:|-----:|-------:|-------:|----------:|
| CreateException_System           |   564.7 ns | 11.19 ns | 10.99 ns |    2 | 0.1402 |      - |    2.3 KB |
| CreateException_Moroshka         |   415.6 ns |  8.25 ns | 12.09 ns |    1 | 0.1378 |      - |   2.26 KB |
| ThrowNestedException_System      | 6,448.6 ns | 65.92 ns | 61.66 ns |    8 | 0.1450 |      - |   2.39 KB |
| ThrowNestedException_Moroshka    | 4,379.4 ns | 51.82 ns | 45.93 ns |    7 | 0.1373 |      - |   2.35 KB |
| ToStringException_System         | 1,454.6 ns | 27.12 ns | 24.04 ns |    4 | 0.2518 |      - |   4.14 KB |
| ToStringException_Moroshka       | 3,520.5 ns | 64.68 ns | 60.51 ns |    6 | 1.0185 | 0.0038 |  16.64 KB |
| ToStringNestedException_System   | 1,111.9 ns | 19.62 ns | 33.31 ns |    3 | 0.3700 |      - |   6.07 KB |
| ToStringNestedException_Moroshka | 3,303.8 ns | 64.76 ns | 66.51 ns |    5 | 0.8354 | 0.0114 |  13.69 KB |
*/
