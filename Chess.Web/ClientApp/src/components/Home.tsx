import React, { Component, useRef, useState } from 'react';
import '../App.css';
import Board from '../components/Board';
import Timer from '../components/Timer'
import Piece from '../Models/Piece';
import playerColor from '../Models/PlayerColor';

export interface SendMoveFunc {
    (code: string): void
}

interface HomeProps {

}

interface HomeState {
    pieces: Piece[],
    result: string
}

export interface RefTimer {
    setTime: (milliseconds: number) => void;
    setIsPause: (value: boolean) => void;
}

const Home = (props: HomeProps) => {

    //let whiteTimer = useRef<RefTimer>();
    //let blackTimer = useRef<RefTimer>();

    let [state, setState] = useState<HomeState>({ pieces: null, result: "" })

    async function restartGame() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
        };

        await fetch('chess/restart', requestOptions);
        populatePieces();
    }

    async function populatePieces() {
        const response = await fetch('chess/getpieces');
        const data = await response.json();

        handleResponse(data, false);
    }

    async function sendMove(code) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(code)
        };

        const response = await fetch('chess/makemove', requestOptions);
        const data = await response.json();


        handleResponse(data, true);
    }

    function handleResponse(data, runTimer) {
        let result = "";

        if (data.isEnd == true) {
            if (data.isCheck == true) {
                result = data.currentPlayer === playerColor.white ? "0-1" : "1-0";
            }
            else {
                result = "1/2-1/2"
            }
        }

        if (data.isEnd == true && data.isCheck != true) {
            setState({ pieces: state.pieces, result: "1/2-1/2" })
        }


        setState({ pieces: data.pieces, result: result });

        const isWhite = data.currentPlayer === playerColor.white;
        //blackTimer.current.setTime(data.remainTimes[playerColor.black]);
        //whiteTimer.current.setTime(data.remainTimes[playerColor.white]);
        //blackTimer.current.setIsPause(isWhite || !runTimer || data.isEnd);
        //whiteTimer.current.setIsPause(!isWhite || !runTimer || data.isEnd);
    }

    if (state.pieces == null) {
        populatePieces();
        return (<h1>loading...</h1>)
    }
    else {
        return (
            <div>
                <button onClick={restartGame}>Restart</button>
                <div>
                    <Board pieces={state.pieces} sendMove={sendMove}></Board>
                    {/*<div className="timerDiv">*/}
                    {/*    <Timer ref={blackTimer} />*/}
                    {/*    <Timer ref={whiteTimer} />*/}
                    {/*</div>*/}
                    <div className="gameResult">
                        {state.result}
                    </div>
                </div>
            </div>

        );
    }
}

export default Home