import React from 'react';

const UserLabel = ({user, color}) => {
    return (
        <div className={`user-label users__user-label user-label_${color.name}`}>
            <p> {user.name} </p>
        </div>
    );
};

export default UserLabel;