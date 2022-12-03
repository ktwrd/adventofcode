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

function chunkArray(arr, len) {
    let ret = []
    for (let i = 0; i < arr.length; i += len) {
        ret.push(arr.slice(i, i + len))
    }
    return ret;
}

let charmap = {}
let groupArr = chunkArray(data, 3)
for (let group of groupArr) {
    let groupCharMap = {}
    for (let rucksack of group) {
        let firstCompartment = rucksack.substring(0, rucksack.length / 2)
        let secondCompartment = rucksack.substring(rucksack.length / 2)
        let localcount = {}
    
        for (let fc of firstCompartment) {
            if (localcount[fc] == undefined)
                localcount[fc] = [0,0]
            localcount[fc][0]++
        }
        for (let sc of secondCompartment) {
            if (localcount[sc] == undefined)
                localcount[sc] = [0,0]
            localcount[sc][1]++
        }
        // Only allow items where it is inside of the 1st and 2nd compartment.
        // localcount = Object.fromEntries(Object.entries(localcount).filter(v => v[1][0] > 0 && v[1][1] > 0))
    
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
        console.log(sortedPriorityMap)
        for (let item of sortedPriorityMap) {
            if (groupCharMap[item.char] == undefined) {
                groupCharMap[item.char] = 0
            }
            groupCharMap[item.char] += 1
        }
    }
    console.log(groupCharMap)
    for (let pair of Object.entries(groupCharMap)) {
        if (charmap[pair[0]] == undefined) {
            charmap[pair[0]] = 0
        }
        if (pair[1] == 3)
            charmap[pair[0]] += getPriority(pair[0])
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
