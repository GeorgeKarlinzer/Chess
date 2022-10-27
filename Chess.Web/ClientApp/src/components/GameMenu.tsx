import React, { useState } from 'react';
import ApplicationPaths from '../ApplicationPaths';
import './GameMenu.css'

class ClockSetting {
    pref: string;
    clock: string;
    time: number;
    bonus: number;

    constructor(time: number, bonus: number) {
        if (time < 3)
            this.pref = "Bullet";
        else if (time < 10)
            this.pref = "Blitz";
        else if (time < 30)
            this.pref = "Rapid";
        else
            this.pref = "Classic";

        this.clock = `${time}+${bonus}`;
        this.time = time;
        this.bonus = bonus;
    }
}
const clocks = [
    new ClockSetting(1, 0),
    new ClockSetting(2, 1),
    new ClockSetting(3, 0),
    new ClockSetting(3, 2),
    new ClockSetting(5, 0),
    new ClockSetting(5, 3),
    new ClockSetting(10, 0),
    new ClockSetting(10, 5),
    new ClockSetting(15, 10),
    new ClockSetting(30, 0),
    new ClockSetting(30, 20),
]

const GameMenu = () => {

    var [selectedClock, setSelectedClock] = useState<ClockSetting>(null);

    async function searchGame(clock: ClockSetting) {
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

    }

    return (
        <div className="gamePools">
            {clocks.map(x => {
                if (x == selectedClock)
                    return (
                        <div key={x.clock} onClick={stopSearch} className="test" >
                            <div className="lds-roller"><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>
                        </div>
                    )
                return (
                    <div key={x.clock} onClick={() => searchGame(x)} >
                        <div className="clock noselect">{x.clock}</div>
                        <div className="pref noselect">{x.pref}</div>
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