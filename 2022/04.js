const fs = require('fs')
const { EOL } = require('os')

const lines = fs.readFileSync('04.txt')
    .toString()
    .split(EOL)

function calculateGroups() {
    let tempgroup = []
    let ret = []
    for (let i of lines) {
        ret.push(i.split(','))
    }
    return ret
}

const groups = calculateGroups()

function parseGroup(group) {
    let items = []
    for (let line of group) {
        let c = line.split('-')
        items.push([
            parseInt(c[0]),
            parseInt(c[1])
        ])
    }
    return items
}

let globalCount = 0
let groupTable = []
for (let groupLineIndex in groups) {
    let groupLine = groups[groupLineIndex]
    let group = parseGroup(groupLine)
    for (let innerIndex in group) {
        let inner = group[innerIndex]
        let bk = false
        for (let outerIndex in group) {
            let outer = group[outerIndex]
            if (outerIndex != innerIndex &&
                inner[0] >= outer[0] &&
                inner[0] <= outer[1] &&
                inner[1] <= outer[1] &&
                inner[1] >= outer[0]) {
                    groupTable.push({
                        groupLineIndex,
                        innerIndex,
                        outerIndex
                    })
                globalCount++;
                bk = true;
                break;
            }
        }
        if (bk)
            break;
    }
}
console.table(groupTable)
console.log(`total: ${globalCount}`)