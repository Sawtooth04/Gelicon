import React from 'react';
import {useNavigate} from "react-router-dom";

const Login = () => {
    const navigate = useNavigate();

    function responseHandler(response) {
        navigate("/");
    }

    async function formSubmitHandle(event) {
        event.preventDefault();
        let data = (new FormData(document.getElementById('login-form')));
        let response = await fetch('/login/login', {
            method: "POST",
            body: data
        });
        responseHandler(response);
    }

    return (
        <div>
            <form id="login-form" onSubmit={formSubmitHandle}>
                <p> Email: </p>
                <input type="text" name="email"/>
                <p> Password: </p>
                <input type="text" name="password"/>
                <button type="submit"> Submit </button>
            </form>
        </div>
    );
};

export default Login;