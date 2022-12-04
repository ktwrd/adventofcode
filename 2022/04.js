const fs = require('fs')
const { EOL } = require('os')

const lines = fs.readFileSync('04-sample.txt')
    .toString()
    .split(EOL)

function calculateGroups() {
    let tempgroup = []
    let groups = []
    for (let i of lines) {
        if (i.length < 1) {
            groups.push(tempgroup)
        } else {
            tempgroup.push(i)
        }
    }
    return groups
}

const groups = calculateGroups()
