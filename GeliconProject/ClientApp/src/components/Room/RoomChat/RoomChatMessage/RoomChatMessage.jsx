import React from 'react';

const RoomChatMessage = ({message, color}) => {
    return (
        <div className={`messages__message message message_${color.name}`}>
            <div className="message__meta meta">
                <p className="meta__owner"> {message.sender.name} </p>
                <p className="meta__time"> {message.time} </p>
            </div>
            <p className="message__text"> {message.message} </p>
        </div>
    );
};

export default RoomChatMessage;