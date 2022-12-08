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
 * @property {Number[][]} raw
 * @property {Number} width
 * @property {Number} height
 */
/**
 * @returns {GeneratedGrid}
 */
function generate() {
    let data = {
        trees: []
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
        }
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
            count++;
        else
        {
            // top
            if (data.trees.filter(v => v.x == tree.x && v.y < tree.y).every(v => prec(v, tree)))
                count++;
            // bottom
            else if (data.trees.filter(v => v.x == tree.x && v.y > tree.y).every(v => prec(v, tree)))
                count++;
            // left
            else if (data.trees.filter(v => v.y == tree.y && v.x < tree.x).every(v => prec(v, tree)))
                count++;
            // right
            else if (data.trees.filter(v => v.y == tree.y && v.x > tree.x).every(v => prec(v, tree)))
                count ++
        }
    }
    console.log(count)
}
let startTimestamp = Date.now()
let generated = generate()
iterCount(generated)
console.log(`${Date.now() - startTimestamp}ms`)