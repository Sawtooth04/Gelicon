import React from 'react';
import Button from "../../../UI/Button/Button";

const RoomChatMessage = ({message, color, deleteMessage}) => {
    function onDeleteClick() {
        deleteMessage(message);
    }

    return (
        <div className={`messages__message message message_${color.name}`}>
            <div className="message__meta meta">
                <p className="meta__owner"> {message.sender.name} </p>
                <p className="meta__time"> {message.time} </p>
            </div>
            <p className="message__text"> {message.message} </p>
            <div className="message__controls">
                <Button onClick={onDeleteClick} src={"/source/images/delete-music.png"}/>
            </div>
        </div>
    );
};

export default RoomChatMessage;