const fs = require('fs')
const { EOL } = require('os')

let startTimestamp = Date.now()

const file = fs.readFileSync('05.txt').toString()

/**
 * @typedef instruction
 * @property {Number} count - amount of things to remove from `0 to n`
 * @property {Number} sourceIndex - column to take off the top
 * @property {Number} targetIndex - column to move to (insert before `0`th item)
 */

function parseColumnCount(headerlines)
{
    let finalLine = headerlines[headerlines.length - 1]
    let splittedFinalLine = finalLine.trim().split('   ')
    let totalLength = parseInt(splittedFinalLine[splittedFinalLine.length - 1])
    return totalLength
}
function parsetable()
{
    let splitted = file.split(EOL)
    let headerlines = []
    for (let line of splitted)
    {
        if (line.length < 1)
            break;
        // add padding so each column is 4 chars exactly
        headerlines.push(line += ' ')
    }
    let columnCount = parseColumnCount(headerlines)

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
    console.log('original stack')
    console.table(letterTable.map(v => v.join(', ')))
    return letterTable
}
/**
 * @summary
 * the array returned is sorted from the top to the bottom of the pile (0: top, len: bottom)
 */
let table = parsetable()
function getInstructionLines() {
    let lines = []
    let foundblank = false
    let filelines = file.split(EOL)
    for (let l of filelines)
    {
        if (l.length < 3)
            foundblank = true

        if (foundblank)
        {
            lines.push(l)
        }
    }
    return lines
}
/**
 * 
 * @returns {instruction[]}
 */
function parseinstructions()
{
    let instructionLines = getInstructionLines()

    let instructions = []
    for (let item of instructionLines)
    {
        if (item.length < 3)
            continue;
        let splitted = item.split(' ')
        let count = parseInt(splitted[1])
        let sourceIndex = parseInt(splitted[3]) - 1
        let targetIndex = parseInt(splitted[5]) - 1
        instructions.push({
            count,
            sourceIndex,
            targetIndex
        })
    }
    return instructions;
}

/**
 * @param {instruction[]} instruction 
 * @param {string[]} stack 
 * @returns {string[]} - modified stack
 */
function executeInstruction(instruction, st)
{
    let stack = JSON.parse(JSON.stringify(st))
    let prepend = []
    let sourceColumn = JSON.parse(JSON.stringify(stack[instruction.sourceIndex]))
    for (let x = 0; x < instruction.count; x++)
    {
        // prepend source column to prepend stack and remove it from the og source column
        prepend = [
            sourceColumn[x],
            ...prepend
        ]
        stack[instruction.sourceIndex].shift()
    }
    // add the prepend stack before the target column data
    stack[instruction.targetIndex] = [
        ...prepend,
        ...stack[instruction.targetIndex]
    ]
    return stack
}


let stackInstructions = parseinstructions()
function printTop(stack)
{
    let items = []
    for (let thing of stack)
    {
        items.push(thing[0])
    }
    console.log(`executed answer: ${items.join('')}`)
}
function exec() {
    let localStack = table
    for (let item of stackInstructions)
    {
        localStack = executeInstruction(item, localStack)
    }
    console.log('executed stack')
    console.table(localStack.map(v => v.join(', ')))
    printTop(localStack)
}
exec()
console.log(`took ${Date.now() - startTimestamp}ms`)