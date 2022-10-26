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
        const newConnection = new HubConnectionBuilder()
            .withUrl(ApplicationPaths.hub)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        fetch(ApplicationPaths.isSearchinGame)
            .then(x => x.text())
            .then(x => setIsSearchingGame(x == 'true'));

        fetch(ApplicationPaths.getGameState, { method: "POST" })
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
                    connection.on('StartGame', x => {
                        setIsInGame(true);
                        getGameState();
                    })
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    function handleGameState(state: GameState) {
        setGameState(state);
        setConverterColor(state.player);
    }

    async function getGameState() {
        var response = await fetch(ApplicationPaths.getGameState, { method: "POST" });
        var data = await response.json();
        handleGameState(data);
    }

    async function searchGame() {
        let response = await fetch(ApplicationPaths.searchGame, { method: "POST" });
        let data = await response.text();
        setIsSearchingGame(data == 'true');
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

    }

    function closeGame() {

    }

    function offerDraw() {

    }

    if (!isInGame) {
        if (isSearchingGame)
            return (<p>Searching game...</p>)

        return (<button onClick={searchGame}>Search game</button>)
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

    return (
        <div>
            <Board pieces={gameState.pieces} sendMove={sendMove}></Board>
            <div className="timerDiv">
                <Timer time={gameState.timersMap[topPlayer].remainMilliseconds} isPaused={!gameState.timersMap[topPlayer].isRunning} />
                <Timer time={gameState.timersMap[botPlayer].remainMilliseconds} isPaused={!gameState.timersMap[botPlayer].isRunning} />
            </div>
            <p className="gameResult">{resultText}</p>
        </div>

    );
}

export default Game