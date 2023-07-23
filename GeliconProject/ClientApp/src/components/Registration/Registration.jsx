import React, {useState} from 'react';
import {NavLink, useNavigate} from "react-router-dom";

const Registration = () => {
    const [emailAvailable, setEmailAvailable] = useState(true);
    const [emailRegex, setEmailRegex] = useState(true);
    const [passwordCorrect, setPasswordCorrect] = useState(true);
    const [usernameCorrect, setUsernameCorrect] = useState(true);
    const navigate = useNavigate();

    function handleErrors(response) {
        let status = response.emailAvailable && response.emailRegex && response.passwordCorrect;
        setEmailAvailable(response.emailAvailable);
        setEmailRegex(response.emailRegex);
        setPasswordCorrect(response.passwordCorrect);
        setUsernameCorrect(response.usernameCorrect);
        if (status)
            navigate("/rooms");
    }

    async function formSubmitHandle(event) {
        event.preventDefault();
        let data = (new FormData(document.getElementById('registration-form')));
        let response = await(await fetch('/registration/register', { method: "POST", body: data })).json();
        handleErrors(response);
    }

    return (
        <div className={"registration"}>
            <div className={"registration__credentials"}>
                <div className={"registration__credentials__logo"}>
                    <img src="/source/images/logo-300x300-2.png"/>
                </div>
                <p className={"registration__credentials__article"}> GELICON </p>
            </div>
            <form id="registration-form" className={"registration__form"} onSubmit={formSubmitHandle}>
                <input className={"registration__form__input"} type="text" name="name" placeholder={"Name"}/>
                <input className={"registration__form__input"} type="text" name="email" placeholder={"Email"}/>
                <input className={"registration__form__input"} type="password" name="password" placeholder={"Password"}/>
                <button className={"registration__form__button"} type="submit"> Submit </button>
                <div className={"registration__validation-errors"}>
                    {usernameCorrect ? null :
                        <p className={"registration__validation-errors__message"}> Incorrect username. </p>
                    }
                    {!usernameCorrect || emailRegex ? null :
                        <p className={"registration__validation-errors__message"}> Email has an incorrect format. </p>
                    }
                    {!usernameCorrect || !emailRegex || emailAvailable ? null :
                        <p className={"registration__validation-errors__message"}> Email unavailable. </p>
                    }
                    {!usernameCorrect || !emailRegex || !emailAvailable || passwordCorrect ? null :
                        <p className={"registration__validation-errors__message"}> Incorrect password. </p>
                    }
                </div>
            </form>
            <div className={"registration__login-invitation"}>
                <p className={"registration__login-invitation__article"}> Already have an account? </p>
                <NavLink className={"registration__login-invitation__link"} to={"/login"}> Log in. </NavLink>
            </div>
        </div>
    );
};

export default Registration;