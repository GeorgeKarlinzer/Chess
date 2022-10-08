import React, { Component } from 'react';
import MoveSquare from './MoveSquare';
import Piece from './Piece';

class Board extends Component {
    static displayName = Board.name;

    constructor(props) {
        super(props);
        this.state = { pieces: [], loading: true, selectedPiece: null };
        this.showPossibleMoves = this.showPossibleMoves.bind(this);
        this.makeMove = this.makeMove.bind(this)
        this.renderBoard = this.renderBoard.bind(this)
        this.renderMoves = this.renderMoves.bind(this)
        this.renderPieces = this.renderPieces.bind(this)
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

    componentDidMount() {
        this.populatePieces();
    }

    static renderPiece(piece, isSelected, callback) {
        const imgName = `${piece.color.toLowerCase()}-${piece.name.toLowerCase()}.svg`;
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

    static renderMove(x, y, callback) {
        return (
            <MoveSquare x={x} y={y} makeMove={callback} />
        )
    }

    renderMoves() {
        let moves = []
        if (this.state.selectedPiece != null)
            moves = this.state.selectedPiece.moves;
        return (
            <>{
                moves.map(m =>
                    Board.renderMove(m.x, m.y, this.makeMove))
            }</>
        )
    }

    renderBoard() {
        return (

            <section className="board">
                {this.renderPieces()}
                {this.renderMoves()}
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

    async makeMove(x, y) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ PieceId: this.state.selectedPiece.id, TargetPos: { X: x, Y: y } })
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

