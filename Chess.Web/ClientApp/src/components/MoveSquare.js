import React, { Component, FC } from 'react';
import styled from 'styled-components';
import convertY from '../Models/CoordConverter'

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

    onClick() {
        this.props.makeMove(this.props.x,this.props.y)
    }

    render() {
        return (
            <StyledSquare
                x={this.props.x}
                y={this.props.y}
                onClick={() => this.onClick()} >
            </StyledSquare>
        )
    }
}

export default MoveSquare;