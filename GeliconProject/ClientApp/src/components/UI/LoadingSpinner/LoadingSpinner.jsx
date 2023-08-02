import React from 'react';

const LoadingSpinner = () => {
    return (
        <div className="loading-spinner">
            <div className={"loading-spinner__wrapper"}>
                <div className="loading-spinner__triangle"/>
            </div>
        </div>
    );
};

export default LoadingSpinner;