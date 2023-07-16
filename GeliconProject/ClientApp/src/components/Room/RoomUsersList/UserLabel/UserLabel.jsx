import React from 'react';

const UserLabel = ({user, color, isOnline}) => {
    return (
        <div className={`user-label users__user-label user-label_${color.name}${isOnline ? ' user-label_online' : ''}`}>
            <p> {user.name} </p>
        </div>
    );
};

export default UserLabel;