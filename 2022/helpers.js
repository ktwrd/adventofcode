module.exports = {
    chunkArray(arr, len) {
        let ret = []
        for (let i = 0; i < arr.length; i += len) {
            ret.push(arr.slice(i, i + len))
        }
        return ret;
    }
}