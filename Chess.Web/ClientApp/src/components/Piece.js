import React, { Component, FC } from 'react';
import styled from 'styled-components';

const StyledSquare = styled.div`
    width: 4rem;
    height: 4rem;
    left: ${({ x }) => 4 * x + 'rem'};
    top: ${({ y }) => 4 * y + 'rem'};
    position:absolute;
    background-image: ${({ img }) => 'url(' + img + ')'};
    background-size: 3.5rem 3.5rem;
    background-repeat: no-repeat;
    background-position: center;
 `


class Piece extends Component {

    constructor(props) {
        super(props)
        console.log(props.moves)
        this.state = { x: props.x, y: props.y, img: props.img, moves: props.moves }
        this.onClick = this.onClick.bind(this)
    }

    onClick() {
        console.log(state.moves)
    }

    render() {
        return (
            <StyledSquare
                x={this.state.x}
                y={this.state.y}
                img={this.state.img}
                onClick={() => this.OnClick(this.state)} >
            </StyledSquare>
        )
    }
}

export default Piece;