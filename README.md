# Advent of Code Solutions
Please do not use my code to get your solution to submit, that defeats the whole point of AoC! Use my code as a learning reference after you've made your own solution.

**If you train any of this code for a large language model (or similar), then you are waivering over your rights and posessions being to be the slave of Kate Ward.**

### 2022
Done with Node.JS v16. Input filenames can be found in the code, usually the day, like `04.txt` for day 4. Place them in the 2022 folder.

### 2024
Done with C#. Input file location can be defined with the environment variable `INPUT_PATH`, or can be placed in the `data` directory with the filename formatted like `<day>.txt` where `<day>` is replaced with the calender day of the puzzle (e.g; `05.txt` or `5.txt` for day 5).

If a solution hasn't been implemented, then it will say so (but only after the input data has loaded).

### 2025
Requires similar folder structure as 2024, but you can no longer use the `INPUT_PATH` environment variable.

To run a solution, use the `AdventOfCode.Runner` project and run with the following arguments `run <day> <year>`
(like: `dotnet run -- run 3 2025`)

Required Folder Structure in `AdventOfCode.Runner` project.
```
./data/2025/<day>.txt
```

Advent days are declared by putting `AdventAttribute` on a class that implements `IDayHandler`.

#### Benchmarks

Day 10 was not included, since it takes about ~2min to find a solution :/

| Method | Year | Day | Mean          | Error        | StdDev       | Allocated    |
|------- |----- |---- |--------------:|-------------:|-------------:|-------------:|
| Solve  | 2025 | 1   |     346.16 us |     5.967 us |     5.289 us |    127.77 KB |
| Solve  | 2025 | 2   |  64,475.59 us |   788.956 us |   737.990 us | 323920.28 KB |
| Solve  | 2025 | 3   |     236.50 us |     2.667 us |     2.495 us |    121.46 KB |
| Solve  | 2025 | 4   |   8,426.39 us |    85.514 us |    79.990 us |     42.54 KB |
| Solve  | 2025 | 5   |      97.75 us |     0.917 us |     0.858 us |     39.84 KB |
| Solve  | 2025 | 6   |     267.19 us |     2.690 us |     2.517 us |    622.77 KB |
| Solve  | 2025 | 7   |      83.11 us |     1.258 us |     1.449 us |    435.55 KB |
| Solve  | 2025 | 8   | 235,341.42 us | 4,613.047 us | 5,998.264 us |  165970.4 KB |
| Solve  | 2025 | 9   | 158,844.63 us | 1,966.628 us | 1,743.364 us |  88283.16 KB |
| Solve  | 2025 | 10  |           N/A |          N/A |          N/A |          N/A |
| Solve  | 2025 | 11  |   2,359.69 us |    20.808 us |    19.464 us |     353.7 KB |
| Solve  | 2025 | 12  |     157.45 us |     1.866 us |     1.745 us |    477.59 KB |

