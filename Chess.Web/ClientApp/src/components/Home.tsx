﻿import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import ApplicationPaths from "../ApplicationPaths";
import { CheckAthentication, IsSignedIn } from "./account/AccountManager";
import Game from "./Game";

const Home = () => {
    const navigate = useNavigate();
    let [isLoading, setIsLoading] = useState(true);

    if (isLoading)
        CheckAthentication()
            .then(() => {
                setIsLoading(false);
            });
    if (isLoading)
        return (<h1>Loading...</h1>)
    else if (IsSignedIn())
        return (<Game />);
    else
        navigate(ApplicationPaths.login)
}

export default Home