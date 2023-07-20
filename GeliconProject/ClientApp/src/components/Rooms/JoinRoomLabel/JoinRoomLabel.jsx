import React from 'react';

const JoinRoomLabel = ({onClick}) => {
    return (
        <div className={`rooms__rooms-label rooms-label join-room-label`} onClick={onClick}>
            <img src="/source/images/connect.png"/>
        </div>
    );
};

export default JoinRoomLabel;