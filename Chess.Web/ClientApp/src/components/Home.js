import React, { Component, useState } from 'react';
import '../App.css';
import Board from '../components/Board';
import Timer from '../components/Timer'
import playerColor from '../Models/PlayerColor';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.restartGame = this.restartGame.bind(this);
        this.sendMove = this.sendMove.bind(this)
        this.handleResponse = this.handleResponse.bind(this);
        this.state = { pieces: [] };
        this.populatePieces();

        this.blackTimer = React.createRef();
        this.whiteTimer = React.createRef();
    }

    async restartGame() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
        };

        await fetch('chess/restart', requestOptions);
        this.populatePieces();
    }

    render() {
        if (this.state == null) {
            return (<h1>loading...</h1>)
        }
        else {
            return (
                <div>
                    <button onClick={this.restartGame}>Restart</button>
                    <div>
                        <Board pieces={this.state.pieces} sendMove={this.sendMove}></Board>
                        <div className="timerDiv">
                            <Timer ref={this.blackTimer} />
                            <Timer ref={this.whiteTimer} />
                        </div>
                    </div>
                </div>

            );
        }
    }

    async populatePieces() {
        const response = await fetch('chess/getpieces');
        const data = await response.json();

        this.handleResponse(data, false);
    }

    async sendMove(code) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(code)
        };

        const response = await fetch('chess/makemove', requestOptions);
        const data = await response.json();

        if (data.isEnd === true) {
            console.log('end')
        }

        if (data.isCheck === true) {
            console.log('check')
        }

        this.handleResponse(data, true);
    }

    handleResponse(data, runTimer) {
        this.blackTimer.current.correct(data.remainTimes[playerColor.black]);
        this.whiteTimer.current.correct(data.remainTimes[playerColor.white]);
        this.setState({ pieces: data.pieces });

        const isWhite = data.currentPlayer === playerColor.white;
        this.blackTimer.current.isPaused = isWhite || !runTimer || data.isEnd;
        this.whiteTimer.current.isPaused = !isWhite || !runTimer || data.isEnd;
    }
}
