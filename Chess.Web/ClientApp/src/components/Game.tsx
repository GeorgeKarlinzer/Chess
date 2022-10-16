import React, { useState } from 'react';
import '../App.css';
import Board from '../components/Board';
import Timer from '../components/Timer';
import Piece from '../Models/Piece';
import playerColor from '../Models/PlayerColor';

export interface SendMoveFunc {
    (code: string): void
}

interface Props {

}

interface State {
    pieces: Piece[],
    result: string,
    blackTime: number,
    whiteTime: number,
    blackIsPaused: boolean,
    whiteIsPaused: boolean
}

const Game = (props: Props) => {

    let [state, setState] = useState<State>({
        pieces: null,
        result: "",
        blackTime: 0,
        whiteTime: 0,
        blackIsPaused: true,
        whiteIsPaused: true
    })

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
                if (data.remainTimes[playerColor.black] == 0) {
                    result = "1-0";
                } else if (data.remainTimes[playerColor.white] == 0) {
                    result = "0-1";
                }
                else
                    result = "1/2-1/2"
            }
        }

        const isWhite = data.currentPlayer === playerColor.white;
        let blackTime = data.remainTimes[playerColor.black]
        let whiteTime = data.remainTimes[playerColor.white]
        let blackIsPause = isWhite || !runTimer || data.isEnd
        let whiteIsPause = !isWhite || !runTimer || data.isEnd

        setState({
            pieces: data.pieces,
            result: result,
            blackTime: blackTime,
            whiteTime: whiteTime,
            blackIsPaused: blackIsPause,
            whiteIsPaused: whiteIsPause
        });
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
                    <div className="timerDiv">
                        <Timer time={state.blackTime} isPaused={state.blackIsPaused} />
                        <Timer time={state.whiteTime} isPaused={state.whiteIsPaused} />
                    </div>
                    <div className="gameResult">
                        {state.result}
                    </div>
                </div>
            </div>

        );
    }
}

export default Game