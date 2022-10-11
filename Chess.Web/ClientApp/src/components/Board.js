import React, { Component } from 'react';
import ChoosePiece from './ChoosePiece';
import MoveSquare from './MoveSquare';
import Piece from './Piece';

class Board extends Component {
    static displayName = Board.name;

    constructor(props) {
        super(props);
        this.state = { selectedPiece: null, isChoosingPiece: false};

        this.showPossibleMoves = this.showPossibleMoves.bind(this);
        this.makeMove = this.makeMove.bind(this)
        this.renderBoard = this.renderBoard.bind(this)
        this.renderMoves = this.renderMoves.bind(this)
        this.renderPieces = this.renderPieces.bind(this)
        this.renderChoosePiece = this.renderChoosePiece.bind(this)
        this.onClick = this.onClick.bind(this)
        this.move = {}
    }

    onClick() {
        if (this.state.selectedPiece != null) {
            this.setState({ isChoosingPiece: false, selectedPiece: null })
        }
    }

    showPossibleMoves(pieceId) {
        if (this.state.selectedPiece != null && pieceId == this.state.selectedPiece.id) {
            this.setState({ selectedPiece: null, isChoosingPiece: false })
            return;
        }

        if (pieceId != null) {
            let piece = this.props.pieces.find(({ id }) => id === pieceId)
            this.setState({ selectedPiece: piece, isChoosingPiece: false })
            return;
        }
    }

    makeMove(code, move, isChoosePiece) {
        this.move = move
        if (isChoosePiece === true) {
            this.setState({ isChoosingPiece: true })
        }
        else {
            this.showPossibleMoves(this.state.selectedPiece.id)
            this.props.sendMove(code);
        }
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
                this.props.pieces.map(p =>
                    Board.renderPiece(p, p === this.state.selectedPiece, this.showPossibleMoves))
            }</>
        )
    }

    static renderMove(x, y, piece, callback) {
        return (
            <MoveSquare x={x} y={y} piece={piece} makeMove={callback} />
        )
    }

    renderMoves() {
        let moves = []
        if (this.state.selectedPiece != null)
            moves = this.state.selectedPiece.moves;
        return (
            <>{
                moves.map(m =>
                    Board.renderMove(m.x, m.y, this.state.selectedPiece, this.makeMove))
            }</>
        )
    }

    renderChoosePiece() {
        if (this.state.isChoosingPiece === true)
            return (
                <ChoosePiece piece={this.state.selectedPiece}
                    makeMove={this.makeMove}
                    x={this.move.x}
                    y={this.move.y}
                ></ChoosePiece>
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
        return (this.renderBoard());
    }
}

export default Board;

