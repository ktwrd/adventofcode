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
