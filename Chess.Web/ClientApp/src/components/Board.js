import React, { Component } from 'react';
import Piece from './Piece';

class Board extends Component {
    static displayName = Board.name;

    constructor(props) {
        super(props);
        this.state = { pieces: [], loading: true };
    }

    componentDidMount() {
        this.populatePieces();
    }

    static renderPiece(piece) {
        const imgName = `${piece.color.toLowerCase()}-${piece.name.toLowerCase()}.png`;
        const img = require('../assets/Pieces/' + imgName);

        return (
            <Piece x={piece.x} y={piece.y} img={img}></Piece>
        )
    }

    static renderBoard(pieces) {
        return (
            <section className="board">
                {pieces.map((p, i) =>
                    Board.renderPiece(p)
                )}
            </section>
        )
    }
                    //{<Piece x={p.x} y={p.y} img={iimg}></Piece>}

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Board.renderBoard(this.state.pieces);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populatePieces() {
        const response = await fetch('chess/getsomenew');
        const data = await response.json();
        this.setState({ pieces: data, loading: false });
    }
}

export default Board;

