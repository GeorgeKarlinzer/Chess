import React, { Component } from 'react';
import styled from 'styled-components';
import { convertY } from '../Models/Converter'

const StyledRectangle = styled.div`
    width: 16rem;
    height: 4rem;
    left: ${({ x }) => 4 * (x - 1.5) + 'rem'};
    top: ${({ y }) => 4 * (convertY(y) - 1) + 'rem'};
    position:absolute;
    background-color: rgb(54, 219, 39, 0.5);
    background-size: 1rem 1rem;
    background-repeat: no-repeat;
    background-position: center;
 `

class ChoosePiece extends Component{
    constructor(props){
        super(props)

    }

    render() {
        return (
            <StyledRectangle x={this.props.x} y={this.props.y} >
                <button className="choosePieceButton">Bishop</button>            
                <button className="choosePieceButton">Knight</button>            
                <button className="choosePieceButton">Queen</button>            
                <button className="choosePieceButton">Rook</button>            
            </StyledRectangle>
        )
    }
}

export default ChoosePiece;