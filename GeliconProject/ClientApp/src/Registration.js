import React from 'react';
import {useNavigate} from "react-router-dom";

const Registration = () => {
    const navigate = useNavigate();

    function responseHandler(response) {
        navigate("/");
    }

    async function formSubmitHandle(event) {
        event.preventDefault();
        let data = (new FormData(document.getElementById('registration-form')));
        let response = await fetch('/registration/register', {
            method: "POST",
            body: data
        });
        responseHandler(response);
    }

    return (
        <div>
            <form id="registration-form" onSubmit={formSubmitHandle}>
                <p> Name: </p>
                <input type="text" name="name"/>
                <p> Password: </p>
                <input type="text" name="password"/>
                <p> Email: </p>
                <input type="text" name="email"/>
                <button type="submit"> Submit </button>
            </form>
        </div>
    );
};

export default Registration;