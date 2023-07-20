import React from 'react';

const UserLabel = ({user, color, isOnline, isEditable, onClickCallback}) => {

    function onClick() {
        onClickCallback(isEditable, user, color);
    }

    return (
        <div className={`user-label users__user-label${isOnline ? ' user-label_online' : ''}`} onClick={onClick}
            style={{color: color, borderColor: color}}>
            <p> {user.name} </p>
        </div>
    );
};

export default UserLabel;