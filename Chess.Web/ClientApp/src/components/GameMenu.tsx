import React, { useEffect, useState } from 'react';
import ApplicationPaths from '../ApplicationPaths';
import { ClockSettings } from './ClockSettings';
import './GameMenu.css'

const clocks = [
    new ClockSettings(1 * 60, 0),
    new ClockSettings(2 * 60, 1),
    new ClockSettings(3 * 60, 0),
    new ClockSettings(3 * 60, 2),
    new ClockSettings(5 * 60, 0),
    new ClockSettings(5 * 60, 3),
    new ClockSettings(10 * 60, 0),
    new ClockSettings(10 * 60, 5),
    new ClockSettings(15 * 60, 10),
    new ClockSettings(30 * 60, 0),
    new ClockSettings(30 * 60, 20),
]

const GameMenu = () => {

    var [selectedClock, setSelectedClock] = useState<ClockSettings>(null);

    useEffect(() => {
        fetch(ApplicationPaths.getSearchingGame)
            .then(x => x.json())
            .then(x => {
                if (x.time == 0 && x.bonus == 0)
                    setSelectedClock(null);
                else
                    setSelectedClock(new ClockSettings(x.time, x.bonus));
            });
    }, [])

    async function searchGame(clock: ClockSettings) {
        const body = JSON.stringify({ time: clock.time, bonus: clock.bonus });
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: body
        };

        let response = await fetch(ApplicationPaths.searchGame, requestOptions);
        let data = await response.text();
        if (data == 'true')
            setSelectedClock(clock);
    }

    async function stopSearch() {
        var response = await fetch(ApplicationPaths.stopSearch, { method: "POST" });
        var data = await response.text();
        if (data == 'true')
            setSelectedClock(null);
    }

    return (
        <div className="gamePools">
            {clocks.map(x => {
                if (JSON.stringify(x) == JSON.stringify(selectedClock))
                    return (
                        <div key={x.getClock()} onClick={stopSearch} >
                            Waiting for opponent...
                        </div>
                    )
                return (
                    <div key={x.getClock()} onClick={() => searchGame(x)} >
                        <div className="clock noselect">{x.getClock()}</div>
                        <div className="pref noselect">{x.getPref()}</div>
                    </div>
                )
            })}
            <div key="Custom" >
                <div className="clock noselect">Custom</div>
            </div>
        </div>
    )
}

export default GameMenu