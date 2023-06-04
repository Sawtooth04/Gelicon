import React, {useState, useEffect} from 'react';
import RoomChatControls from "./RoomChatControls/RoomChatControls";
import RoomChatMessagesList from "./RoomChatMessagesList/RoomChatMessagesList";

const RoomChat = ({connector, roomID, roomUsersColors}) => {
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("AppendMessage", appendMessage);
            connector.addEventHandler("PingReceive", pingReceive);
            connector.addEventHandler("LogPing", logPing);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("AppendMessage", appendMessage);
            connector.removeEventHandler("PingReceive", pingReceive);
            connector.addEventHandler("LogPing", logPing);
        }

        //Handlers
        function appendMessage(message) {
            if (message) {
                message.time = (new Date().toLocaleTimeString());
                if (messages.length >= 100)
                    setMessages([...messages.slice(1, messages.length), message]);
                else
                    setMessages([...messages, message]);
                removeEventHandlers();
            }
        }

        async function pingReceive() {
            await connector.pingResponse(roomID);
        }

        function logPing(ping) {
            console.log(`Current ping: ${ping} ms`)
        }

        if (connector.connected)
            addEventHandlers();
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