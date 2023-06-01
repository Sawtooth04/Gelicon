import React from 'react';
import {Link} from 'react-router-dom';

const Header = (props) => {
    return (
        <header className={"header"}>
            <Link className="header__link" to="/registration"> Registration </Link>
            <Link className="header__link" to="/login"> Login </Link>
        </header>
    );
};

export default Header;