import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { NavItem, NavLink } from 'reactstrap';
import ApplicationPaths from '../../ApplicationPaths';
import { CheckAthentication, IsSignedIn } from './AccountManager';

const AccountNavMenu = () => {
    let [isSignIn, setIsSignIn] = useState(false);

    useEffect(() => {
        CheckAthentication()
            .then(() => setIsSignIn(IsSignedIn()))
    }, [])

    if (!isSignIn)
        return (
            <>
                <NavItem>
                    <NavLink tag={Link} to={ApplicationPaths.loginRoute}>Login</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={Link} to={ApplicationPaths.registerRoute}>Register</NavLink>
                </NavItem>
            </>
        )
    else
        return (
            <>
                <NavItem>
                    <NavLink tag={Link} to={ApplicationPaths.logoutRoute}>Sign out</NavLink>
                </NavItem>
            </>
        )
}

export default AccountNavMenu;