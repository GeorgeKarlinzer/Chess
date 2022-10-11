import React, { Component, useState } from 'react';
import '../App.css';
import Board from '../components/Board';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.restartGame = this.restartGame.bind(this);
        this.sendMove = this.sendMove.bind(this)
        this.state = { pieces: [] };
        this.populatePieces();
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
            return(<h1>loading...</h1>)
        }
        else {
            return (
                <div>
                    <Board pieces={this.state.pieces} sendMove={this.sendMove}></Board>
                    <button onClick={this.restartGame}>Restart</button>
                </div>

            );
        }
    }

    async populatePieces() {
        const response = await fetch('chess/getpieces');
        const data = await response.json();
        this.setState({ pieces: data.pieces });
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

        this.setState({ pieces: data.pieces });
    }
}
