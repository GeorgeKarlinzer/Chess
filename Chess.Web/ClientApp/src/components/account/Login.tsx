import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ApplicationPaths from '../../ApplicationPaths';
import { SetSignIn } from './AccountManager';

const Login = () => {

    const navigate = useNavigate();

    let [username, setUsername] = useState("");
    let [password, setPassword] = useState("");

    let states = {
        ["username"]: setUsername,
        ["password"]: setPassword,
    }

    function submit(e) {
        e.preventDefault();
        tryLogin();
    }

    function onChange(e) {
        const { id, value } = e.target;

        states[id.toString()](value);
    }

    async function tryLogin() {
        const body = {
            "username": username,
            "password": password
        }

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        };

        const response = await fetch(ApplicationPaths.login, requestOptions);
        const data = await response.text();
        if (data == "success") {
            SetSignIn(true);
            navigate("/");
            window.location.reload();
        }
    }

    return (
        <form className="form" onSubmit={(e) => submit(e)}>
            <div className="form-body">
                <div className="username">
                    <label className="form__label" htmlFor="username">Username </label>
                    <input className="form__input" type="text" id="username" value={username} onChange={e => onChange(e)} placeholder="Username" />
                </div>
                <div className="password">
                    <label className="form__label" htmlFor="password">Password </label>
                    <input className="form__input" type="password" id="password" value={password} onChange={e => onChange(e)} placeholder="Password" />
                </div>
            </div>
            <div className="footer">
                <button type="submit" className="btn">Login</button>
            </div>
        </form>
    );
}

export default Login;