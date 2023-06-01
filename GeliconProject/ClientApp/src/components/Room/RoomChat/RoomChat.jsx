import React, {useState, useEffect} from 'react';
import RoomChatControls from "./RoomChatControls/RoomChatControls";
import RoomChatMessagesList from "./RoomChatMessagesList/RoomChatMessagesList";

const RoomChat = ({connector, roomID, roomUsersColors}) => {
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        function appendMessage(message) {
            if (message) {
                message.time = (new Date().toLocaleTimeString());
                if (messages.length >= 100)
                    setMessages([...messages.slice(1, messages.length), message]);
                else
                    setMessages([...messages, message]);
                connector.removeEventHandler("AppendMessage", appendMessage);
            }
        }

        if (connector.connected)
            connector.addEventHandler("AppendMessage", appendMessage);
    }, [connector, connector.connected, messages]);

    async function sendMessage(message) {
        await connector.sendMessage(message, roomID);
    }

    return (
        <div className={"room__chat chat"}>
            <RoomChatMessagesList messages={messages} roomUsersColors={roomUsersColors}/>
            <RoomChatControls sendMessage={sendMessage}/>
        </div>
    );
};

export default RoomChat;