import React from 'react';
import {Link} from 'react-router-dom';

const Header = (props) => {
    return (
        <header className={"header"}>
            <div className="header__content">
                <div className="header__content__logo">
                    <img src="/source/images/logo-300x300.png"/>
                </div>
                <p className="header__content__article"> GELICON </p>
                <div className="header__links">
                    <div className="header__links__link">
                        <Link to="/registration"> Registration </Link>
                    </div>
                    <div className="header__links__link">
                        <Link to="/login"> Login </Link>
                    </div>
                </div>
            </div>
        </header>
    );
};

export default Header;