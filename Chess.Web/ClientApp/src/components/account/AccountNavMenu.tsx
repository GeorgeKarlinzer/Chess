import React from 'react';
import { Link } from 'react-router-dom';
import { NavItem, NavLink } from 'reactstrap';
import ApplicationPaths from '../../ApplicationPaths';
import { IsSignedIn } from './AccountManager';

const AccountNavMenu = () => {
    if (!IsSignedIn())
        return (
            <>
                <NavItem>
                    <NavLink tag={Link} to={ApplicationPaths.login}>Login</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={Link} to={ApplicationPaths.register}>Register</NavLink>
                </NavItem>
            </>
        )
    else
        return (
            <>
                <NavItem>
                    <NavLink tag={Link} to={'~/' + ApplicationPaths.logout}>Sign out</NavLink>
                </NavItem>
            </>
        )
}

export default AccountNavMenu;