import React from 'react';
import styled from 'styled-components';
import { convertY } from '../Models/Converter';
import { ClickOnPieceFunc } from './Board';

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

interface PieceProps {
    id: number,
    x: number,
    y: number,
    img: any,
    clickOnPiece: ClickOnPieceFunc,
    isSelected: boolean
}

const Piece = (props: PieceProps) => {

    function onClick(e: React.MouseEvent<HTMLElement>) {
        e.stopPropagation();
        props.clickOnPiece(props.id)
    }

    return (
        <StyledSquare className={props.isSelected ? "selected" : ""}
            id={props.id}
            x={props.x}
            y={props.y}
            img={props.img}
            onClick={(e) => onClick(e)} >
        </StyledSquare>
    )
}

export default Piece;