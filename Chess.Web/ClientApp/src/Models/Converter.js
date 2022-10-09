﻿export function convertY(y) {
    return 7 - y;
}

export function toChessPos(x, y) {
    const firstCoordsMap = {
        0: 'a',
        1: 'b',
        2: 'c',
        3: 'd',
        4: 'e',
        5: 'f',
        6: 'g',
        7: 'h'
    }

    return firstCoordsMap[x] + (y + 1);
}