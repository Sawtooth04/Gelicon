import React, {useState} from 'react';
import {Route, Navigate} from 'react-router-dom';
import ContentRoutes from "../ContentRoutes/ContentRoutes";

const PrivateRoute = ({ element, redirectElement, ...props }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(true);

    async function onNavigate() {
        let response = await fetch("/authorization/authorization-check", { method: 'get' });
        setIsLoggedIn(response.ok);
    }

    return isLoggedIn ? <ContentRoutes onNavigate={onNavigate}/> : <Navigate to={"/login"} replace/>
};

export default PrivateRoute;