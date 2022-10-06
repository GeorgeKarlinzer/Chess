import React, { FC } from 'react';
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

const Piece = ({ x, y, img }) => {
    return (
        <StyledSquare x={x} y={y} img={img}></StyledSquare>
    )
}

export default Piece;