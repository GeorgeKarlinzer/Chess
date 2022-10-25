import playerColor from "./PlayerColor";

export function setConverterColor(c: playerColor): void {
    color = c;
}

var color = playerColor.white;

export function convertY(y: number): number {
    return color == playerColor.white ? 7 - y : y;
}

export function toChessPos(x: number, y: number): string {
    let firstCoordsMap = {
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
