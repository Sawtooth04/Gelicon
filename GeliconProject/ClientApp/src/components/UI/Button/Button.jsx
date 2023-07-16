import React from 'react';

const Button = ({className, onClick, ...props}) => {
    return (
        <button className={`icon-button${(typeof(className) != 'undefined') ? ' ' + className : ''}`} onClick={onClick}>
            <img {...props} alt={""}/>
        </button>
    );
};

export default Button;