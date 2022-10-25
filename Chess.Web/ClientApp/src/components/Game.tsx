import React, { useEffect, useState } from 'react';
import '../App.css';
import Board from '../components/Board';
import Timer from '../components/Timer';
import Piece from '../Models/Piece';
import playerColor from '../Models/PlayerColor';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import ApplicationPaths from '../ApplicationPaths';
import gameStatus from '../Models/GameStatus';
import { setConverterColor } from '../Models/Converter';

export interface SendMoveFunc {
    (code: string): void
}

interface Props {

}

interface TimerDto {
    remainMilliseconds: number,
    isRunning: boolean
}

interface GameState {
    pieces: Piece[],
    isCheck: boolean,
    status: gameStatus
    currentPlayer: number,
    timersMap: { [key in playerColor]: TimerDto }
}

const Game = (props: Props) => {

    let [connection, setConnection] = useState<HubConnection>(null);
    let [isInGame, setIsInGame] = useState(false);
    let [color, setColor] = useState<playerColor>(null)
    let [gameState, setGameState] = useState<GameState>(null)

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7130/chesshub')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    connection.on('UpdateBoard', x => {
                        setGameState(JSON.parse(x))
                    });
                    connection.on('StartGame', color => {
                        setColor(color);
                        setConverterColor(color);
                        setIsInGame(true);
                        getGameState();
                    })
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    async function getGameState() {
        var response = await fetch(ApplicationPaths.getGameState, { method: "POST" });
        var data = await response.json();
        setGameState(data);
    }

    async function searchGame() {
        const options = {
            headers: { 'Content-Type': 'application/json' },
            method: "POST"
        }
        let response = await fetch("chess/searchgame", options);

        let data = await response.text();

        if (data == 'true')
            console.log('searching');
        else
            console.log('cannot search');
    }

    function sendMove(code) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(code)
        };

        fetch('chess/makemove', requestOptions);
    }

    if (!isInGame)
        return (<button onClick={searchGame}>Search game</button>)

    if (gameState == null)
        return (<h1>Loading...</h1>)

    let timer;

    if (color == playerColor.white)
        timer = (
            <div className="timerDiv">
                <Timer time={gameState.timersMap[playerColor.black].remainMilliseconds} isPaused={!gameState.timersMap[playerColor.black].isRunning} />
                <Timer time={gameState.timersMap[playerColor.white].remainMilliseconds} isPaused={!gameState.timersMap[playerColor.white].isRunning} />
            </div>
        )
    else
        timer = (
            <div className="timerDiv">
                <Timer time={gameState.timersMap[playerColor.white].remainMilliseconds} isPaused={!gameState.timersMap[playerColor.white].isRunning} />
                <Timer time={gameState.timersMap[playerColor.black].remainMilliseconds} isPaused={!gameState.timersMap[playerColor.black].isRunning} />
            </div>
        )
    console.log(gameState.timersMap);
    return (
        <div>
            <div>
                <Board pieces={gameState.pieces} sendMove={sendMove}></Board>
                {timer}
            </div>
        </div>

    );
}

export default Game