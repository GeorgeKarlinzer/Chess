import React, { Component } from 'react';
import styled from 'styled-components';
import { convertY, toChessPos } from '../Models/Converter'

const StyledRectangle = styled.div`
    width: 16rem;
    height: 4rem;
    left: ${({ x }) => 4 * (x - 1.5) + 'rem'};
    top: ${({ y }) => 4 * (convertY(y) - 1) + 'rem'};
    position:absolute;
 `

class ChoosePiece extends Component{
    constructor(props){
        super(props)
        this.onClick = this.onClick.bind(this)
    }

    onClick(e, pieceCode) {
        e.stopPropagation();
        const piece = this.props.piece;
        const sourcePos = toChessPos(piece.position.x, piece.position.y);
        const targetPos = toChessPos(this.props.x, this.props.y);
        const code = sourcePos + targetPos + pieceCode;
        let isChoosePiece = false;

        this.props.makeMove(code, null, isChoosePiece);
    }

    render() {
        return (
            <StyledRectangle x={this.props.piece.position.x} y={this.props.piece.position.y} >
                <button className="choosePieceButton" onClick={(e) => this.onClick(e, 'B')}>Bishop</button>            
                <button className="choosePieceButton" onClick={(e) => this.onClick(e, 'N')}>Knight</button>            
                <button className="choosePieceButton" onClick={(e) => this.onClick(e, 'Q')}>Queen</button>            
                <button className="choosePieceButton" onClick={(e) => this.onClick(e, 'R')}>Rook</button>            
            </StyledRectangle>
        )
    }
}

export default ChoosePiece;