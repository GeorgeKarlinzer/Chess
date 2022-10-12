import React, { Component } from 'react';
import styled from 'styled-components';
import { convertY, toChessPos } from '../Models/Converter'
import Piece from '../Models/Piece';
import playerColor from '../Models/PlayerColor';
import { MakeMoveFunc } from './Board';

const StyledSquare = styled.div`
    width: 4rem;
    height: 4rem;
    left: ${({ x }) => 4 * x + 'rem'};
    top: ${({ y }) => 4 * convertY(y) + 'rem'};
    position:absolute;
    background-color: rgb(54, 219, 39, 0.5);
    background-size: 1rem 1rem;
    background-repeat: no-repeat;
    background-position: center;
 `

interface MoveSquareProps {
    x: number,
    y: number,
    piece: Piece,
    makeMove: MakeMoveFunc
}

const MoveSquare = (props: MoveSquareProps) => {
    function onClick(e) {
        e.stopPropagation();
        const piece = props.piece;
        const sourcePos = toChessPos(piece.position.x, piece.position.y);
        const targetPos = toChessPos(props.x, props.y);
        const code = sourcePos + targetPos;
        let isChoosePiece = false;

        const promotionY = piece.color === playerColor.white ? 7 : 0;
        if (props.y === promotionY && piece.name === "pawn") {
            isChoosePiece = true;
        }

        props.makeMove(code, { x: props.x, y: props.y }, isChoosePiece);
    }

    return (
        <StyledSquare
            x={props.x}
            y={props.y}
            onClick={(e) => onClick(e)} >
        </StyledSquare>
    )
}

export default MoveSquare;