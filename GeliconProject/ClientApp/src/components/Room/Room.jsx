import React, {useEffect, useState} from 'react';
import {useParams} from "react-router-dom";
import RoomChat from "./RoomChat/RoomChat";
import HubConnector from "../../Utils/HubConnector";
import RoomUsersList from "./RoomUsersList/RoomUsersList";
import RoomMusic from "./RoomMusic/RoomMusic";

const Room = () => {
    const [connector, setConnector] = useState(new HubConnector());
    const {roomID} = useParams();
    const [users, setUsers] = useState([]);
    const [roomUsersColors, setRoomUsersColors] = useState([]);
    const [onlineCheckInterval, setOnlineCheckInterval] = useState(null);
    const [onlineUsers, setOnlineUsers] = useState([]);

    useEffect(() => {
        return () => connector.disconnect();
    }, []);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetOnlineUsers", setOnlineUsers);
        }

        function onUpdate() {
            if (onlineCheckInterval !== null && typeof(onlineCheckInterval) !== 'undefined')
                clearInterval(onlineCheckInterval);
            connector.removeEventHandler("SetOnlineUsers", setOnlineUsers);
        }

        async function getRoom() {
            return await fetch("/room/join", {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify([Number(roomID)])
            });
        }

        function setStates(room) {
            setUsers(room.users);
            setRoomUsersColors(room.roomUsersColors);
        }

        const init = async () => {
            addEventHandlers();
            await connector.getOnlineUsersList();
            setStates(await (await getRoom()).json());
            setOnlineCheckInterval(setInterval(() => connector.getOnlineUsersList(), 15000));
        };

        connector.setConnectionToHub(roomID).then(() => void init());
        return onUpdate;
    }, [connector]);

    return (
        <div className="room">
            <RoomMusic connector={connector}/>
            <RoomChat connector={connector} roomID={roomID} roomUsersColors={roomUsersColors}/>
            <RoomUsersList users={users} roomUsersColors={roomUsersColors} onlineUsers={onlineUsers} className="room__users users"/>
        </div>
    );
};

export default Room;