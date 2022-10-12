import React from 'react';
import styled from 'styled-components';
import { convertY, toChessPos } from '../Models/Converter';
import Piece from '../Models/Piece';
import { MakeMoveFunc } from './Board';

const StyledRectangle = styled.div`
    width: 16rem;
    height: 4rem;
    left: ${({ x }) => 4 * (x - 1.5) + 'rem'};
    top: ${({ y }) => 4 * (convertY(y) - 1) + 'rem'};
    position:absolute;
 `

interface IChoosePieceProps {
    piece: Piece,
    x: number,
    y: number,
    makeMove: MakeMoveFunc
}

const ChoosePiece = (props: IChoosePieceProps) => {

    function onClick(e: React.MouseEvent<HTMLElement>, pieceCode: string) {
        e.stopPropagation();
        const piece: Piece = props.piece;
        const sourcePos = toChessPos(piece.position.x, piece.position.y);
        const targetPos = toChessPos(props.x, props.y);
        const code = sourcePos + targetPos + pieceCode;
        let isChoosePiece = false;

        props.makeMove(code, null, isChoosePiece);
    }

    return (
        <StyledRectangle x={props.piece.position.x} y={props.piece.position.y} >
            <button className="choosePieceButton" onClick={(e) => onClick(e, 'B')}>Bishop</button>
            <button className="choosePieceButton" onClick={(e) => onClick(e, 'N')}>Knight</button>
            <button className="choosePieceButton" onClick={(e) => onClick(e, 'Q')}>Queen</button>
            <button className="choosePieceButton" onClick={(e) => onClick(e, 'R')}>Rook</button>
        </StyledRectangle>
        )
}

export default ChoosePiece;