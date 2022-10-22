import React from 'react';
import ApplicationPaths from '../../ApplicationPaths';
import { useNavigate } from 'react-router-dom';
import { SetSignIn } from './AccountManager';

const Logout = () => {
    console.log('render logout')
    const navigate = useNavigate();

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify("")
    };

    fetch(ApplicationPaths.logout, requestOptions)
        .then(() => {
            SetSignIn(false);
            navigate("/");
        });

    return (<h1>Loging out...</h1>)
}

export default Logout;