const fs = require('fs')
const { EOL } = require('os')

let data = fs.readFileSync('03.txt')
    .toString()
    .split(EOL)


let priorityMap = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'
function getPriority(char) {
    let idx = priorityMap.indexOf(char)
    if (idx < 0)
        return 0;
    return idx + 1;
}

let charmap = {}
for (let rucksack of data) {
    let firstCompartment = rucksack.substring(0, rucksack.length / 2)
    let secondCompartment = rucksack.substring(rucksack.length / 2)
    let localcount = {}

    for (let fc of firstCompartment) {
        if (localcount[fc] == undefined)
            localcount[fc] = [0,0]
        localcount[fc][0]++
    }
    for (let sc of secondCompartment) {
        if (localcount[sc] != undefined)
            localcount[sc][1]++
    }
    // Only allow items where it is inside of the 1st and 2nd compartment.
    localcount = Object.fromEntries(Object.entries(localcount).filter(v => v[1][0] > 0 && v[1][1] > 0))

    let localPriorityCharMap = []
    for (let pair of Object.entries(localcount)) {
        localPriorityCharMap.push({
            char: pair[0],
            count: pair[1],
            priority: getPriority(pair[0])
        })
    }

    // Sort based on priority (lowest to highesty)
    let sortedPriorityMap = localPriorityCharMap.sort((a, b) => a.priority - b.priority)

    if (sortedPriorityMap.length > 0) {
        let item = sortedPriorityMap[0]
        if (charmap[item.char] == undefined) {
            charmap[item.char] = 0
        }
        charmap[item.char] += item.priority
    }
}
// Remove items that have no count.
charmap = Object.fromEntries(Object.entries(charmap).filter(v => v[1] > 0))


let total = 0
for (let pair of Object.entries(charmap)) {
    console.log(`${pair[0]}: ${pair[1]}`)
    total += pair[1]
}
console.log(`total: ${total}`)
