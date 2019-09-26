# matlab-dotNet-benchmark

Please run the C# test program first (matlab-dotNet-benchmark\dotNetClass\TestProgram\bin\Release) to get some numbers and then matlab-dotNet-benchmark\DotNetBenchmark.m to see the differences.

Three things are tested:
1) polling data with netObj.getData()
2) polling data with getData(netObj), matlab only
3) event driven data

All tests run 10 seconds each.

