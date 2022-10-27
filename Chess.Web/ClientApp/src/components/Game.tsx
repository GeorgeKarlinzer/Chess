import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import React, { useEffect, useState } from 'react';
import '../App.css';
import ApplicationPaths from '../ApplicationPaths';
import Board from '../components/Board';
import Timer from '../components/Timer';
import { setConverterColor } from '../Models/Converter';
import gameStatus from '../Models/GameStatus';
import Piece from '../Models/Piece';
import playerColor from '../Models/PlayerColor';
import GameMenu from './GameMenu';

export interface SendMoveFunc {
    (code: string): void
}

interface TimerDto {
    remainMilliseconds: number,
    isRunning: boolean
}

interface GameState {
    pieces: Piece[],
    isCheck: boolean,
    status: gameStatus
    player: playerColor,
    timersMap: { [key in playerColor]: TimerDto }
}

const Game = () => {

    let [connection, setConnection] = useState<HubConnection>(null);
    let [isSearchingGame, setIsSearchingGame] = useState(false);
    let [isInGame, setIsInGame] = useState(false);
    let [gameState, setGameState] = useState<GameState>(null)

    useEffect(() => {
        fetch(ApplicationPaths.isSearchinGame)
            .then(x => x.text())
            .then(x => setIsSearchingGame(x == 'true'));
    }, [isInGame])

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(ApplicationPaths.hub)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        fetch(ApplicationPaths.isSearchinGame)
            .then(x => x.text())
            .then(x => setIsSearchingGame(x == 'true'));

        fetch(ApplicationPaths.getGameState)
            .then(x => x.json())
            .then(x => {
                if ('player' in x) {
                    setIsInGame(true);
                    handleGameState(x);
                }
            })
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    connection.on('UpdateBoard', x => {
                        handleGameState(JSON.parse(x));
                    });
                    connection.on('StartGame', () => {
                        setIsInGame(true);
                        getGameState();
                    })
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    function handleGameState(state: GameState) {
        try {
            setGameState(state);
            setConverterColor(state.player);
        }
        catch { }
    }

    async function getGameState() {
        var response = await fetch(ApplicationPaths.getGameState);
        var data = await response.json();
        handleGameState(data);
    }

    function sendMove(code) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(code)
        };

        fetch(ApplicationPaths.makeMove, requestOptions);
    }

    function resign() {
        fetch(ApplicationPaths.resign, { method: "POST" });
    }

    function closeGame() {
        fetch(ApplicationPaths.closeGame, { method: "POST" })
            .then(() => setIsInGame(false));
    }

    function offerDraw() {

    }

    if (!isInGame) {
        return (<GameMenu></GameMenu>)
    }

    if (gameState == null)
        return (<h1>Loading...</h1>)

    let botPlayer = gameState.player;
    let topPlayer = botPlayer == playerColor.white ? playerColor.black : playerColor.white;

    let resultText: string;
    switch (gameState.status) {
        case gameStatus.blacksWon: resultText = "Blacks won";
            break;
        case gameStatus.whitesWon: resultText = "Whites won";
            break;
        case gameStatus.draw: resultText = "Draw";
            break;
        case gameStatus.inProgress: resultText = "";
            break;
    }

    const closeGameBtn = gameState.status != gameStatus.inProgress ? (<button onClick={closeGame}>Close game</button>) : (<></>)
    const resultP = gameState.status != gameStatus.inProgress ? (<p>{resultText}</p>) : (<></>)

    return (
        <div>
            {closeGameBtn}
            <Board pieces={gameState.pieces} sendMove={sendMove}></Board>
            <div className="functionBoard">
                <div className="timerDiv">
                    <Timer time={gameState.timersMap[topPlayer].remainMilliseconds} isPaused={!gameState.timersMap[topPlayer].isRunning} />
                    <Timer time={gameState.timersMap[botPlayer].remainMilliseconds} isPaused={!gameState.timersMap[botPlayer].isRunning} />
                </div>
                <div className="bttnContainer">
                    <button className="funcBttn" onClick={resign}>Resign</button>
                    <button className="funcBttn">Offer a draw</button>
                </div>
                {resultP}
            </div>
        </div>
    );
}

export default Game