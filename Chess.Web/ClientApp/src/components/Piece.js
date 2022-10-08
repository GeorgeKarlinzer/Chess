﻿import React, { Component, FC } from 'react';
import styled from 'styled-components';
import convertY from '../Models/CoordConverter'

const StyledSquare = styled.div`
    width: 4rem;
    height: 4rem;
    left: ${({ x }) => 4 * x + 'rem'};
    top: ${({ y }) => 4 * convertY(y) + 'rem'};
    position:absolute;
    background-image: ${({ img }) => 'url(' + img + ')'};
    background-size: 4rem 4rem;
    background-repeat: no-repeat;
    background-position: center;
 `


class Piece extends Component {

    constructor(props) {
        super(props)
        this.state = { x: props.x, y: props.y, img: props.img }
        this.onClick = this.onClick.bind(this)
    }

    onClick() {
        this.props.showPossibleMoves(this.props.id)
    }

    render() {
        return (
            <StyledSquare className={this.props.isSelected ? "selected" : ""}
                id={this.props.id}
                x={this.state.x}
                y={this.state.y}
                img={this.state.img}
                onClick={() => this.onClick()} >
            </StyledSquare>
        )
    }
}

export default Piece;