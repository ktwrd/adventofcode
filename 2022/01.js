const fs = require('fs')
const os = require('os')

let content = fs.readFileSync('01.txt').toString()

let splitted = content.split(os.EOL)
let chunked = []
let working = []
for (let i = 0; i < splitted.length; i++) {
    let item = splitted[i]
    if (item.length < 1) {
        chunked.push(working)
        working = []
    } else {
        working.push(item)
    }
}

let countmap = []
let highestCountIndex = -1;
let highestCount = 0;
for (let i = 0; i < chunked.length; i++) {
    let numchunk = chunked[i].map(v => parseInt(v))
    let sum = numchunk.reduce((a, b) => a + b)
    if (sum > highestCount) {
        highestCountIndex = i;
        highestCount = sum
    }
    countmap.push({
        count: sum,
        index: i
    })
}
console.log(`Highest count index: ${highestCountIndex}`)
console.log(`Count: ${highestCount}`)

let countmapSorted = countmap.sort((a,b) => b.count - a.count)
let topthree = 0
for (let i = 0; i < 3; i++) {
    console.log(`${i+1}: ${countmapSorted[i].count}`)
    topthree += countmapSorted[i].count
}
console.log(`Top 3 has ${topthree} calories`)