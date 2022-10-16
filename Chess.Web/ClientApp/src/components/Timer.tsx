import React, { useEffect, useState } from 'react';

interface TimerProps {
    isPaused: boolean,
    time: number
}

const Timer = (props: TimerProps) => {
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
        if (!props.isPaused) {
            timer = setTimeout(() => {
                if (!props.isPaused) setTime(time - 100)
            }, 100);
        }
        return () => clearTimeout(timer)
    })

    return (
        <div className="timer">{formatTime(time)}</div>
    )
}

export default Timer;