import React, { Component } from 'react';
import styled from 'styled-components';
import { convertY, toChessPos } from '../Models/Converter'

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

class MoveSquare extends Component {

    constructor(props) {
        super(props)
        this.onClick = this.onClick.bind(this)
    }

    onClick(e) {
        e.stopPropagation();
        const piece = this.props.piece;
        const sourcePos = toChessPos(piece.position.x, piece.position.y);
        const targetPos = toChessPos(this.props.x, this.props.y);
        const code = sourcePos + targetPos;
        let isChoosePiece = false;

        const promotionY = piece.color === "white" ? 7 : 0;
        if (this.props.y === promotionY && piece.name === "pawn") {
            isChoosePiece = true;
        }

        this.props.makeMove(code, isChoosePiece);
    }

    render() {
        return (
            <StyledSquare
                x={this.props.x}
                y={this.props.y}
                onClick={(e) => this.onClick(e)} >
            </StyledSquare>
        )
    }
}

export default MoveSquare;