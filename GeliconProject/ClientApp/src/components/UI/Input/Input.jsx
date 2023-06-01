import React from 'react';

const Input = ({children, props}) => {
    return (
        <div className="input">
            <p className="input__label"> {children} </p>
            <input className="input__input" {...props}/>
        </div>
    );
};

export default Input;