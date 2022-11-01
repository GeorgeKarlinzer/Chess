import React, { useEffect, useState } from 'react';
import ApplicationPaths from '../ApplicationPaths';
import playerColor from '../Models/PlayerColor';

interface TimerProps {
    isPaused: boolean,
    time: number,
    color: playerColor
}

const Timer = (props: TimerProps) => {
    if (Math.random() < 0.01) {
        console.log('sync timer with server');
        fetch(`${ApplicationPaths.getTime}?player=${props.color}`)
            .then(x => x.json())
            .then(x => {
                if (x.remainMilliseconds != -1)
                    setTime(x.remainMilliseconds);
                console.log('synced timer')
            })

    }

    let [time, setTime] = useState(props.time);
    let [lastSync, setLastSync] = useState(props.time);

    if (lastSync != props.time) {
        setLastSync(props.time)
        setTime(props.time)
    }

    function formatTime(milliseconds: number) {
        const minutes = (milliseconds / 60000) >> 0;
        const seconds = ((milliseconds / 1000) >> 0) % 60;

        return `${(minutes / 10 >> 0)}${minutes % 10}:${(seconds / 10) >> 0}${seconds % 10}`;
    }

    useEffect(() => {
        let timer: NodeJS.Timeout
        const timeStamp = Date.now();
        if (!props.isPaused) {
            timer = setTimeout(() => {
                setTime(time - (Date.now() - timeStamp))
            }, 100);
        }
        return () => clearTimeout(timer)
    })

    return (
        <div className="timer">{formatTime(time)}</div>
    )
}

export default Timer;