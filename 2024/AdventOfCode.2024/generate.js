const fs = require('fs');
if (process.argv.length < 3)
{
    console.error("Specify day as argument");
    process.exit(1);
    return;
}

var d = parseInt(process.argv[2]);

var content = [
    `using System.ComponentModel;`,
    ``,
    `namespace AdventOfCode.TwentyTwentyFour;`,
    ``,
    `[DefaultValue(${d})]`,
    `public class Day${d} : IDayHandler`,
    `{`,
    `    public void Run(string[] inputData)`,
    `    {`,
    `        var a = PartOne(inputData);`,
    `        Console.WriteLine($"Part One: {a}");`,
    `        var b = PartTwo(inputData);`,
    `        Console.WriteLine($"Part Two: {b}");`,
    `    }`,
    ``,
    `    private long PartOne(string[] inputData)`,
    `    {`,
    `        return 0;`,
    `    }`,
    ``,
    `    private long PartTwo(string[] inputData)`,
    `    {`,
    `        return 0;`,
    `    }`,
    `}`
];

const outputFilename = `Day${d}.cs`;
fs.writeFileSync(outputFilename, content.join('\n'));
console.log('Wrote content to: ' + outputFilename);