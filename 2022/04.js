const fs = require('fs')
const { EOL } = require('os')

const lines = fs.readFileSync('04.txt')
    .toString()
    .split(EOL)
    .filter(v => v.length > 0)


function calculateGroups() {
    let ret = []
    for (let i of lines) {
        if (i.length > 1)
            ret.push(i.split(','))
    }
    return ret
}

const groups = calculateGroups()

function parseGroup(group) {
    let ret = []
    for (let item of group) {
        let c = item.split('-')
        ret.push([
            parseInt(c[0]),
            parseInt(c[1])
        ])
    }
    return ret
}


let totalCount = 0
let overlapCount = 0
for (let groupLineIndex in groups) {
    let group = parseGroup(groups[groupLineIndex])

    let a = group[0]
    let b = group[1]

    if (a[0] >= b[0] && a[1] <= b[1])
        totalCount += 1
    else if (b[0] >= a[0] && b[1] <= a[1])
        totalCount += 1

    if (b[0] <= a[0] && a[0] <= b[1])
        overlapCount += 1
    else if (a[0] <= b[0] && b[0] <= a[1])
        overlapCount += 1
}

console.log(`${totalCount}`)
console.log(`${overlapCount}`)