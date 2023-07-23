import React, {useState} from 'react';
import {NavLink, useNavigate} from "react-router-dom";

const Login = () => {
    const [emailValid, setEmailValid] = useState(true);
    const [passwordValid, setPasswordValid] = useState(true);
    const navigate = useNavigate();

    function validationErrorsHandle(response) {
        setEmailValid(response.emailValid);
        setPasswordValid(response.passwordValid);
    }

    async function formSubmitHandle(event) {
        event.preventDefault();
        let data = (new FormData(document.getElementById('login-form')));
        let response = await (await fetch('/login/login', { method: "POST", body: data })).json();

        if (Object.keys(response).length === 0)
            navigate("/rooms");
        else
            validationErrorsHandle(response);
    }

    return (
        <div className={"login"}>
            <div className={"login__credentials"}>
                <div className={"login__credentials__logo"}>
                    <img src="/source/images/logo-300x300-2.png"/>
                </div>
                <p className={"login__credentials__article"}> GELICON </p>
            </div>
            <form id="login-form" className={"login__form"} onSubmit={formSubmitHandle}>
                <input className={"login__form__input"} type="text" name="email" placeholder={"Email"}/>
                <input className={"login__form__input"} type="password" name="password" placeholder={"Password"}/>
                <div className={"login__validation-errors"}>
                    {emailValid ? null :
                        <p className={"login__validation-errors__message"}> Incorrect email. Please, check the entered data! </p>
                    }
                    {passwordValid ? null :
                        <p className={"login__validation-errors__message"}> Incorrect password. Please, check the entered data! </p>
                    }
                </div>
                <button className={"login__form__button"} type="submit"> Log in </button>
            </form>
            <div className={"login__registration-invitation"}>
                <p className={"login__registration-invitation__article"}> Don't you have an account? </p>
                <NavLink className={"login__registration-invitation__link"} to={"/registration"}> Register a new one. </NavLink>
            </div>
        </div>
    );
};

export default Login;