import React, { Component, Ref, useImperativeHandle, useState } from 'react';
import { RefTimer } from './Home';


const Timer = (props, ref: Ref<RefTimer>) => {

    let [time, setTime] = useState(0);
    let isPaused = false;

    useImperativeHandle(ref, () => ({ setTime: correct, setIsPause: setIsPause }))

    function correct(milliseconds: number) {
        setTime(milliseconds)
    }

    function setIsPause(value: boolean) {
        isPaused = value;
    }

    function formatTime(milliseconds: number) {
        const seconds = (milliseconds / 1000) >> 0;
        return `${(seconds / 60) >> 0}:${seconds % 60}`;
    }

    async function run() {
        const delay = ms => new Promise(res => setTimeout(res, ms));

        let timeStamp = Date.now();
        let delta = 0;
        while (true) {
            await delay(100);
            if (!isPaused) {
                delta = Date.now() - timeStamp;
                setTime(time - delta);
            }
            timeStamp = Date.now();
        }
    }

    run()

    return (
        <div className="timer">{formatTime(time)}</div>
    )
}

export default Timer;