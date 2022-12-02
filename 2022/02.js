const fs = require('fs')
const os = require('os')

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

let data = fs.readFileSync('02.txt')
    .toString()
    .split(os.EOL)
    .filter(v => v.length > 0)

let total = 0
for (let item of data) {
    total += scoretable[item]
}
console.log(`Total: ${total}`)
