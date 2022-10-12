import playerColor from "./PlayerColor";
import Vector2 from "./Vector2";

export default interface Piece {
    id: number
    position: Vector2
    color: playerColor
    name: string
    moves: Vector2[]
}

