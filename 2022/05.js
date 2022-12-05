const fs = require('fs')
const { EOL } = require('os')

const file = fs.readFileSync('05.txt').toString()
function parseColumnCount(headerlines) {
    let finalLine = headerlines[headerlines.length - 1]
    let splittedFinalLine = finalLine.trim().split('   ')
    let totalLength = parseInt(splittedFinalLine[splittedFinalLine.length - 1])
    return totalLength
}
function parsetable() {
    let splitted = file.split(EOL)
    let headerlines = []
    for (let line of splitted)
    {
        if (line.length < 1)
            break;
        // add padding so each column is 4 chars exactly
        headerlines.push(line += ' ')
    }
    console.log(headerlines)
    let columnCount = parseColumnCount(headerlines)
    console.log(`${columnCount} columns`)


    let letterTable = []
    for (let line of headerlines)
    {
        let stack = []
        let currentStack = ''
        for (let i = 0; i < line.length; i++)
        {
            if (currentStack.length == 4) {
                stack.push(currentStack)
                currentStack = line[i]
            }
            else
                currentStack += line[i]
        }
        if (currentStack.length == 4)
            stack.push(currentStack)
        for (let idx in stack)
        {
            let item = stack[idx]
            if (!item.includes('['))
                continue;

            if (letterTable[idx] == undefined)
                letterTable[idx] = []

            // parse the char
            let trimmed = item.trim()

            // ignore empty column
            if (trimmed.length < 1)
                continue;

            // remove brackets
            trimmed = trimmed.substring(1).substring(0,1)
            letterTable[idx].push(trimmed)
        }
    }
    console.table(letterTable.map(v => v.join(', ')))
    return letterTable
}
parsetable()