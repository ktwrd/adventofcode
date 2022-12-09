const fs = require('fs')
const {EOL} = require('os') 

const data = fs.readFileSync('09.txt').toString()
const lineArray = data.split('\n')

let offset = {
    L: [-1,  0],
    R: [ 1,  0],
    U: [ 0, 1],
    D: [ 0, -1]
}

let head = {
    x: 0,
    y: 0
}
let tail = {
    x: 0,
    y: 0
}
function calc(tailLength)
{
    let seen = {}
    /** @type {Number[]} */
    let head = []
    for (let i = 0; i < tailLength; i++)
    {
        head.push([0, 0])
    }
    for (let line of lineArray)
    {
        let split = line.split(' ')
        let direction = split[0]
        let count = parseInt(split[1])
    
        for (let i = 0; i < count; i++)
        {
            switch (direction)
            {
                case 'R':
                    head[0][1]++
                    break;
                case 'L':
                    head[0][1]--;
                    break;
                case 'U':
                    head[0][0]--;
                    break;
                case 'D':
                    head[0][0]++;
                    break;
            }
            seen[`${head[head.length - 1][0]} ${head[head.length -1][1]}`] = null;

            for (let x = 1; x < head.length; x++)
            {
                if (Math.abs(head[x - 1][0] - head[x][0]) >= 2 || Math.abs(head[x - 1][1] - head[x][1]) >= 2)
                {
                    if (head[x - 1][1] == head[x][1])
                        head[x][0] += head[x - 1][0] > head[x][0] ? 1 : -1
                    else if (head[x - 1][0] == head[x][0])
                        head[x][1] += head[x - 1][1] > head[x][1] ? 1 : -1
                    else
                    {
                        head[x][0] += head[x - 1][0] > head[x][0] ? 1 : -1
                        head[x][1] += head[x - 1][1] > head[x][1] ? 1 : -1
                    }
                }
            }
        }
    }
    return Object.entries(seen).length
}
console.log(`p1: ${calc(2)}`)
console.log(`p2: ${calc(10) + 1}`)