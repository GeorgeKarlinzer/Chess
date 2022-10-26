import React from 'react';
import ApplicationPaths from '../../ApplicationPaths';
import { useNavigate } from 'react-router-dom';
import { SetSignIn } from './AccountManager';

const Logout = () => {
    console.log('render logout')
    const navigate = useNavigate();

    fetch(ApplicationPaths.logout, { method: "POST" })
        .then(() => {
            SetSignIn(false);
            navigate("/");
            window.location.reload();
        });

    return (<h1>Loging out...</h1>)
}

export default Logout;