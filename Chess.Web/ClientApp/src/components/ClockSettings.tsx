export class ClockSettings {
    time: number;
    bonus: number;

    constructor(time: number, bonus: number) {
        this.time = time;
        this.bonus = bonus;
    }

    getClock() {
        return `${this.time / 60}+${this.bonus}`;
    }

    getPref() {
        if (this.time < 3 * 60)
            return "Bullet";

        if (this.time < 10 * 60)
            return "Blitz";

        if (this.time < 30 * 60)
            return "Rapid";

        return "Classic";
    }
}
