const fs = require('fs')
const data = fs.readFileSync('06.txt').toString().trim()
let start = Date.now()
function isUnique(arr) {
    let map = {}
    for (let item of arr) {
        if (map[item] == undefined)
            map[item] = 0
        map[item]++
    }
    for (let pair of Object.entries(map))
        if (pair[1] > 1)
            return false
    return true
}
function execute(size) {
    for (let i = 0; i < data.length; i++)
    {
        let set = data.substring(i, i+size)
        if (isUnique(set))
        {
            return i + size
        }
    }
}
console.log(execute(4))
console.log(execute(14))
console.log(`took ${Date.now() - start}ms`)