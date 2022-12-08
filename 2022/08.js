const fs = require('fs')
const {EOL} = require('os') 

const data = fs.readFileSync('08.txt').toString()
const lineArray = data.split('\n')

/**
 * @typedef {Object} TreeObject
 * @property {Number} x
 * @property {Number} y
 * @property {Number} height
 */
/**
 * @typedef {Object} GeneratedGrid
 * @property {TreeObject[]} trees
 */
/**
 * @returns {GeneratedGrid}
 */
function generate() {
    let data = {
        trees: [],
        width: 0,
        height: 0
    }
    for (let y in lineArray)
    {
        y = parseInt(y.toString())
        let row = lineArray[y].toString().trim()
        if (row.length < 1)
            continue;
        for (let x = 0; x < row.length; x++)
        {
            let height = parseInt(row[x]);
            if (height.toString() != 'NaN')
            {
                data.trees.push({
                    x,
                    y,
                    height
                })
            }
            if (data.width < x)
                data.width = x
        }
        if (data.height < y)
            data.height = y
    }
    return data
}
/**
 * @param {GeneratedGrid} data 
 * @returns {Number} - Amount of visible trees from the outside
 */
function iterCount(data)
{
    let count = 0

    function prec(v, t)
    {
        return v.height < t.height
    }

    for (let tree of data.trees)
    {
        if (tree.x == 0 || tree.y == 0)
        {        
            count++;
        }
        else
        {
            let ts = data.trees.filter(v => v.x == tree.x && v.y < tree.y)
            let bs = data.trees.filter(v => v.x == tree.x && v.y > tree.y)
            let ls = data.trees.filter(v => v.y == tree.y && v.x < tree.x)
            let rs = data.trees.filter(v => v.y == tree.y && v.x > tree.x)
            // top
            if (ts.every(v => prec(v, tree)))
            {
                count++;
            }
            // bottom
            else if (bs.every(v => prec(v, tree)))
            {
                count++;
            }
            // left
            else if (ls.every(v => prec(v, tree)))
            {
                count++;
            }
            // right
            else if (rs.every(v => prec(v, tree)))
            {
                count++;
            }
        }
    }
    return count
}
function secondaryCount(data)
{
    let count = 0
    for (let tree of data.trees)
    { 
        let left = 0
        let right = 0
        let top = 0
        let bottom = 0

        let x = tree.x - 1
        let y = tree.y

        // search left
        while (x >= 0)
        {
            let t = data.trees.filter(v => v.x == x && v.y == y)[0]
            left++

            if (t.height >= tree.height)
                break;
            x--;
        }

        x = tree.x + 1

        // search right
        while (x <= data.width)
        {
            let t = data.trees.filter(v => v.x == x && v.y == y)[0]
            right++

            if (t.height >= tree.height)
                break;
            x++
        }

        x = tree.x
        y = tree.y - 1

        // search up
        while (y >= 0)
        {
            let t = data.trees.filter(v => v.x == x && v.y == y)[0]
            top++

            if (t.height >= tree.height)
                break;
            y--;
        }

        y = tree.y + 1

        // search down
        while (y <= data.height)
        {
            let t = data.trees.filter(v => v.x == x && v.y == y)[0]
            bottom++

            if (t.height >= tree.height)
                break;
            y++;
        }

        count = Math.max(count, left * right * top * bottom)
    }
    return count
}
let startTimestamp = Date.now()
let generated = generate()
let p1 = Date.now()
console.log(`p1: ${iterCount(generated)} (${Date.now() - p1}ms)`)
let p2 = Date.now()
console.log(`p2: ${secondaryCount(generated)} (${((Date.now() - p2) / 1000).toFixed(3)}s)`)

console.log(`${((Date.now() - startTimestamp) / 1000).toFixed(3)}s`)