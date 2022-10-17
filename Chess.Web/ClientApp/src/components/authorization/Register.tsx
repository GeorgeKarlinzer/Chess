import React, { useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import './RegisterStyle.css'


const Register = () => {

    let [username, setUsername] = useState("");
    let [email, setEmail] = useState("");
    let [password, setPassword] = useState("");
    let [password2, setPassword2] = useState("");
    let [errors, setErrors] = useState({});

    let states = {
        ["username"]: setUsername,
        ["email"]: setEmail,
        ["password"]: setPassword,
        ["password2"]: setPassword2
    }

    function submit(e) {
        e.preventDefault();
        let errors: {[key: string] : string} = {}

        if (password !== password2)
            errors["password2"] = "Passwords are different"

        if (!validateEmail(email))
            errors["email"] = "Email is incorrect"

        setErrors(errors);
        if (Object.keys(errors).length === 0) {
            tryRegisterUser();
        }
    }

    function onChange(e) {
        const { id, value } = e.target;

        states[id.toString()](value);
    }

    async function tryRegisterUser() {
        console.log('asd')
        const body = {
            "username": username,
            "email": email,
            "password": password
        }

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        };

        const response = await fetch('authorization/register', requestOptions);
        const data = await response.json();
    }

    return (
        <form className="form" onSubmit={(e) => submit(e)}>
            <div className="form-body">
                <div className="username">
                    <label className="form__label" htmlFor="username">Username </label>
                    <input className="form__input" type="text" id="username" value={username} onChange={e => onChange(e)} placeholder="UserName" />
                    <span className="error">{errors["username"]}</span>
                </div>
                <div className="email">
                    <label className="form__label" htmlFor="email">Email </label>
                    <input type="email" id="email" className="form__input" value={email} onChange={e => onChange(e)} placeholder="Email" />
                    <span className="error">{errors["email"]}</span>
                </div>
                <div className="password">
                    <label className="form__label" htmlFor="password">Password </label>
                    <input className="form__input" type="password" id="password" value={password} onChange={e => onChange(e)} placeholder="Password" />
                </div>
                <div className="confirm-password">
                    <label className="form__label" htmlFor="password2">Confirm Password </label>
                    <input className="form__input" type="password" id="password2" value={password2} onChange={e => onChange(e)} placeholder="Confirm Password" />
                    <span className="error">{errors["password2"]}</span>
                </div>
            </div>
            <div className="footer">
                <button type="submit" className="btn">Register</button>
            </div>
        </form>
    );

    function validateEmail(email) {
        return String(email)
            .toLowerCase()
            .match(
                /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            );
    };
}

export default Register;