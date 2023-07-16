import React, {useState, useEffect} from 'react';
import RoomChatControls from "./RoomChatControls/RoomChatControls";
import RoomChatMessagesList from "./RoomChatMessagesList/RoomChatMessagesList";

const RoomChat = ({connector, roomID, roomUsersColors}) => {
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("AppendMessage", appendMessage);
            connector.addEventHandler("PingReceive", pingReceive);
            connector.addEventHandler("DeleteMessage", deleteMessage);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("AppendMessage", appendMessage);
            connector.removeEventHandler("PingReceive", pingReceive);
            connector.removeEventHandler("DeleteMessage", deleteMessage);
        }

        //Handlers
        function appendMessage(message) {
            if (message) {
                message.time = (new Date().toLocaleTimeString());
                if (messages.length >= 100)
                    setMessages([...messages.slice(1, messages.length), message]);
                else
                    setMessages([...messages, message]);
            }
        }

        function deleteMessage(key) {
            let newMessagesList = [...messages];
            newMessagesList.splice(newMessagesList.findIndex((message) => message.key === key), 1);
            setMessages(newMessagesList);
        }

        async function pingReceive() {
            await connector.pingResponse();
        }

        if (connector.connected)
            addEventHandlers();
        return () => removeEventHandlers();
    }, [connector, connector.connected, messages]);

    async function sendMessage(message) {
        await connector.sendMessage(message, roomID);
    }

    async function deleteMessage(message) {
        await connector.deleteMessage(message.key, roomID);
    }

    return (
        <div className={"room__chat chat"}>
            <RoomChatMessagesList messages={messages} roomUsersColors={roomUsersColors} deleteMessage={deleteMessage}/>
            <RoomChatControls sendMessage={sendMessage}/>
        </div>
    );
};

export default RoomChat;