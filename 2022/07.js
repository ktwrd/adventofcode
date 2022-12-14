const fs = require('fs')
const {EOL} = require('os') 

const data = fs.readFileSync('07.txt').toString()
const lineArray = data.split(EOL)


let listDirectory = false
let root = {
    files: {},
    dirs: {}
}
let working = root

for (let line of lineArray)
{
    if (line.length < 1)
        continue;
    let splitted = line.split(' ')
    if (listDirectory)
    {
        if (line.startsWith('$'))
            listDirectory = false
        else
        {
            if (splitted[0] != 'dir')
            {
                let size = parseInt(splitted[0])
                working.files[splitted[1]] = size
            }
            continue;
        }
    }
    if (splitted[0] == '$')
    {
        // Enable `ls` mode
        if (splitted[1] == 'ls')
        {
            listDirectory = true
            continue;
        }
        else if (splitted[1] == 'cd')
        {
            if (splitted[2] == '..')
            {
                working = working.parent
            }
            else if (splitted[2] == '/')
            {
                working = root
            }    
            else
            {
                if (!working.dirs[splitted[2]]) {
                    working.dirs[splitted[2]] = {
                        parent: working,
                        files: {},
                        dirs: {}
                    }
                }
                working = working.dirs[splitted[2]]
            }
        }
    }
}

function firstPart(item) {
    let size = 0
    let answer = 0

    for (let file of Object.entries(item.files)) {
        size += item.files[file[0]]
    }

    for (let targetDirectory of Object.entries(item.dirs)) {
        let re = firstPart(item.dirs[targetDirectory[0]])
        size += re[0]
        answer += re[1]
    }

    if (size <= 100000) {
        answer += size
    }

    return [size, answer]
}
function secondPart(item) {
    let size = 0
    let returnValue = []

    for (let file of Object.entries(item.files)) {
        size += item.files[file[0]]
    }

    for (let directory of Object.entries(item.dirs)) {
        let directorySize = secondPart(item.dirs[directory[0]])
        size += directorySize[directorySize.length - 1]
        returnValue = returnValue.concat(directorySize)
    }

    returnValue.push(size)
    return returnValue
}

let partOneAnswer = firstPart(root)
console.log([
    '================ part 1',
    partOneAnswer[1]
].join('\n'))


console.log('================ part 2')
let secondSize = secondPart(root)
let totalSecond = secondSize[secondSize.length - 1]
let freespacerequire = totalSecond - 40000000
secondSize.sort((a, b) => a - b)
let done = false
for (let size of secondSize) {
    if (size >= freespacerequire) {
        console.log(size)
        done = true
        break
    }
}
if (!done)
    console.log(-1)
