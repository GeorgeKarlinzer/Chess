import React, { Component, useState } from 'react';
import Vector2 from '../Models/Vector2';
import ChoosePiece from './ChoosePiece';
import MoveSquare from './MoveSquare';
import Piece from '../Models/Piece';
import PieceComponent from './Piece'
import { SendMoveFunc } from './Game';

export interface MakeMoveFunc {
    (code: string, move: Vector2, isChoosePiece: boolean): void
}

export interface ClickOnPieceFunc {
    (pieceId: number): void
}


interface BoardProps {
    pieces: Piece[]
    sendMove: SendMoveFunc
}

interface BoardState {
    selectedPiece: Piece
    isChoosingPiece: boolean
}

export const Board = (props: BoardProps) => {
    let [state, setState] = useState<BoardState>({ selectedPiece: null, isChoosingPiece: false })

    function onClick() {
        if (state.selectedPiece != null) {
            setState({ isChoosingPiece: false, selectedPiece: null })
        }
    }

    function clickOnPiece(pieceId: number) {
        if (state.selectedPiece != null && pieceId === state.selectedPiece.id) {
            setState({ selectedPiece: null, isChoosingPiece: false })
            return;
        }

        if (pieceId != null) {
            let piece = props.pieces.find(({ id }) => id === pieceId)
            setState({ selectedPiece: piece, isChoosingPiece: false })
            return;
        }
    }

    function makeMove(code: string, move: Vector2, isChoosePiece: boolean) {
        if (isChoosePiece == true) {
            setState({ selectedPiece: state.selectedPiece, isChoosingPiece: true })
        }
        else {
            clickOnPiece(state.selectedPiece.id)
            props.sendMove(code);
        }
    }

    function renderPiece(piece: Piece, isSelected: boolean) {
        const imgName = `${piece.color}-${piece.name}.svg`;
        const img = require('../assets/Pieces/' + imgName);

        return (
            <PieceComponent id={piece.id} x={piece.position.x} y={piece.position.y} img={img}
                clickOnPiece={clickOnPiece} isSelected={isSelected} />
        )
    }

    function renderPieces() {
        return (
            <>{
                props.pieces.map(p =>
                    renderPiece(p, p == state.selectedPiece))
            }</>
        )
    }

    function renderMove(x, y, piece) {
        return (
            <MoveSquare x={x} y={y} piece={piece} makeMove={makeMove} />
        )
    }

    function renderMoves() {
        let moves: Vector2[] = []
        if (state.selectedPiece != null)
            moves = state.selectedPiece.moves;
        return (
            <>{
                moves.map(m =>
                    renderMove(m.x, m.y, state.selectedPiece))
            }</>
        )
    }

    function renderChoosePiece() {

        return null;
    }

    const renderBoard = () => {
        return (
            <section className="board" onClick={onClick}>
                {renderPieces()}
                {renderMoves()}
                {renderChoosePiece()}
            </section>
        )
    }

    return (renderBoard())
}

export default Board;