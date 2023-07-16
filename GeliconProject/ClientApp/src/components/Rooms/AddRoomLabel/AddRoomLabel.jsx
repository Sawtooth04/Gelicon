import React from 'react';

const AddRoomLabel = ({onClick}) => {
    return (
        <div className={`rooms__rooms-label rooms-label add-room-label`} onClick={onClick}>
            <img src="/source/images/add-room.png"/>
        </div>
    );
};

export default AddRoomLabel;