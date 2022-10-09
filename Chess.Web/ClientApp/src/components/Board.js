﻿import React, { Component } from 'react';
import ChoosePiece from './ChoosePiece';
import MoveSquare from './MoveSquare';
import Piece from './Piece';

class Board extends Component {
    static displayName = Board.name;

    constructor(props) {
        super(props);
        this.state = { pieces: [], loading: true, selectedPiece: null, isChoosingPiece: false };

        this.showPossibleMoves = this.showPossibleMoves.bind(this);
        this.makeMove = this.makeMove.bind(this)
        this.sendMove = this.sendMove.bind(this)
        this.renderBoard = this.renderBoard.bind(this)
        this.renderMoves = this.renderMoves.bind(this)
        this.renderPieces = this.renderPieces.bind(this)
        this.renderChoosePiece = this.renderChoosePiece.bind(this)
        this.onClick = this.onClick.bind(this)
    }

    onClick() {
        if (this.state.isChoosingPiece === true) {
            this.setState({ isChoosingPiece: false })
        }

        if (this.state.selectedPiece != null) {
            this.setState({ selectedPiece: null })
        }
    }

    showPossibleMoves(pieceId) {
        if (this.state.selectedPiece != null && pieceId == this.state.selectedPiece.id) {
            this.setState({ selectedPiece: null })
            return;
        }

        if (pieceId != null) {
            let piece = this.state.pieces.find(({ id }) => id === pieceId)
            this.setState({ selectedPiece: piece })
            return;
        }
    }

    makeMove(x, y, pieceName, pieceColor) {
        if (pieceName == "pawn" && y == (pieceColor == "white" ? 7 : 0)) {
            this.setState({ isChoosingPiece: true })
            return;
        }
        this.sendMove(x, y, "");
    }

    componentDidMount() {
        this.populatePieces();
    }

    static renderPiece(piece, isSelected, callback) {
        const imgName = `${piece.color}-${piece.name}.svg`;
        const img = require('../assets/Pieces/' + imgName);

        return (
            <Piece id={piece.id} x={piece.position.x} y={piece.position.y} img={img}
                showPossibleMoves={callback} isSelected={isSelected} />
        )
    }

    renderPieces() {
        return (
            <>{
                this.state.pieces.map(p =>
                    Board.renderPiece(p, p === this.state.selectedPiece, this.showPossibleMoves))
            }</>
        )
    }

    static renderMove(x, y, pieceName, pieceColor, callback) {
        return (
            <MoveSquare x={x} y={y} pieceName={pieceName} pieceColor={pieceColor} makeMove={callback} />
        )
    }

    renderMoves() {
        let moves = []
        if (this.state.selectedPiece != null)
            moves = this.state.selectedPiece.moves;
        return (
            <>{
                moves.map(m =>
                    Board.renderMove(m.x, m.y, this.state.selectedPiece.name,
                        this.state.selectedPiece.color, this.makeMove))
            }</>
        )
    }

    renderChoosePiece() {
        if (this.state.isChoosingPiece === true)
            return (
                <ChoosePiece x={this.state.selectedPiece.position.x} y={this.state.selectedPiece.position.x} ></ChoosePiece> 
            )
        else
            return (<></>)
    }

    renderBoard() {
        return (

            <section className="board" onClick={(e) => this.onClick(e)}>
                {this.renderPieces()}
                {this.renderMoves()}
                {this.renderChoosePiece()}
            </section>
        )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderBoard();

        return (
            <div>
                {contents}
            </div>
        );
    }

    async sendMove(x, y, parameter) {
        const body = {
            PieceId: this.state.selectedPiece.id,
            TargetPos: { X: x, Y: y },
            Parameter: parameter
        }

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        };

        const response = await fetch('chess/makemove', requestOptions);
        const data = await response.json();
        this.showPossibleMoves(this.state.selectedPiece.id);
        this.setState({ pieces: data });
    }

    async populatePieces() {
        const response = await fetch('chess/getpieces');
        const data = await response.json();
        this.setState({ pieces: data, loading: false });
    }
}

export default Board;

