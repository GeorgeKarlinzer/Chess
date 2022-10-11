import React, { Component } from 'react';

class Timer extends Component {

    constructor(props) {
        super(props)
        this.state = { milliseconds: 0 }
        this.isPaused = true;
        this.run = this.run.bind(this);
        this.run();
    }

    correct(milliseconds) {
        this.setState({ milliseconds: milliseconds })
    }

    formatTime(milliseconds) {
        const seconds = parseInt(milliseconds / 1000);
        return `${parseInt(seconds / 60)}:${seconds % 60}`;
    }

    async run() {
        const delay = ms => new Promise(res => setTimeout(res, ms));

        let timeStamp = Date.now();
        let delta = 0;
        while (true) {
            await delay(100);
            if (!this.isPaused) {
                delta = Date.now() - timeStamp;
                this.setState({ milliseconds: this.state.milliseconds - delta });
            }
            timeStamp = Date.now();
        }
    }

    render() {
        let time = this.formatTime(this.state.milliseconds);
        return (
            <div className="timer">{time}</div>
        )
    }
}

export default Timer;