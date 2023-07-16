import React from 'react';
import RoomChatMessage from "../RoomChatMessage/RoomChatMessage";

const RoomChatMessagesList = ({messages, roomUsersColors, deleteMessage}) => {
    function findUserColor(user) {
        return roomUsersColors.find(item => item.userID === user.userID).color;
    }

    return (
        <div className="chat__messages messages">
            {
                messages.map((message) =>
                    <RoomChatMessage message={message} key={message.key} color={findUserColor(message.sender)}
                         deleteMessage={deleteMessage}/>
                )
            }
        </div>
    );
};

export default RoomChatMessagesList;