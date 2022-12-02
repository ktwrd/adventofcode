const fs = require('fs')
const os = require('os')

/**
 * loose:    0
 * draw:     3
 * win:      6
 * 
 * rock:     1
 * paper:    2
 * scissors: 3
 */

let scoretable = {
    'A X': 1 + 3,
    'A Y': 2 + 6,
    'A Z': 3 + 0,
    'B X': 1 + 0,
    'B Y': 2 + 3,
    'B Z': 3 + 6,
    'C X': 1 + 6,
    'C Y': 2 + 0,
    'C Z': 3 + 3
}

let scoretable_second = {
    'A X': 3,
    'A Y': 1 + 3,
    'A Z': 2 + 6,
    'B X': 1 + 0,
    'B Y': 2 + 3,
    'B Z': 3 + 6,
    'C X': 2 + 0,
    'C Y': 3 + 3,
    'C Z': 1 + 6
}

let data = fs.readFileSync('02.txt')
    .toString()
    .split(os.EOL)
    .filter(v => v.length > 0)

let total = 0
for (let item of data) {
    total += scoretable[item]
}
console.log(`Total: ${total}`)

let total_second = 0
for (let item of data) {
    total_second += scoretable_second[item]
}
console.log(`Total: ${total_second} (part 2)`)
