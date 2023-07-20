import React, {useEffect, useState} from 'react';
import {useParams} from "react-router-dom";
import RoomChat from "./RoomChat/RoomChat";
import HubConnector from "../../Utils/HubConnector";
import RoomUsersList from "./RoomUsersList/RoomUsersList";
import RoomMusic from "./RoomMusic/RoomMusic";
import GetRoomJoinTokenDialog from "./GetRoomJoinTokenDialog/GetRoomJoinTokenDialog";

const Room = ({onMount, showRoomJoinToken, closeRoomJoinTokenDialog}) => {
    const [connector, setConnector] = useState(new HubConnector());
    const {roomID} = useParams();
    const [users, setUsers] = useState([]);
    const [roomUsersColors, setRoomUsersColors] = useState([]);
    const [onlineUsers, setOnlineUsers] = useState([]);
    const [editableUsersID, setEditableUsersID] = useState([]);

    useEffect(() => {
        onMount();
        return () => {
            connector.disconnect();
        }
    }, []);

    useEffect(() => {
        const onlineCheckInterval = setInterval(async () => await connector.getOnlineUsersList(), 15000);

        function onUpdate() {
            if (onlineCheckInterval !== null && typeof(onlineCheckInterval) !== 'undefined')
                clearInterval(onlineCheckInterval);
        }

        return onUpdate;
    });

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetOnlineUsers", setOnlineUsers);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetOnlineUsers", setOnlineUsers);
        }

        async function getRoomInfo() {
            return await fetch("/room/join", {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify([Number(roomID)])
            });
        }

        function setStates(roomInfo) {
            setUsers(roomInfo.room.users);
            setRoomUsersColors(roomInfo.room.roomUsersColors);
            setEditableUsersID(roomInfo.editableUsers);
        }

        const init = async () => {
            addEventHandlers();
            await connector.getOnlineUsersList();
            setStates(await (await getRoomInfo()).json());
        };

        if (!connector.connected)
            connector.setConnectionToHub(roomID).then(() => void init());
        return removeEventHandlers;
    }, [connector, connector.connected]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetUsersColors", setUsersColors);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetUsersColors", setUsersColors);
        }

        function setUsersColors(usersColors) {
            setRoomUsersColors(usersColors);
        }

        if (connector.connected)
            addEventHandlers();
        return removeEventHandlers;
    }, [roomUsersColors]);

    return (
        <div className="room">
            {showRoomJoinToken ? <GetRoomJoinTokenDialog roomID={roomID} closeRoomJoinTokenDialog={closeRoomJoinTokenDialog}/> : null}
            <RoomMusic connector={connector}/>
            <RoomChat connector={connector} roomID={roomID} roomUsersColors={roomUsersColors}/>
            <RoomUsersList users={users} roomUsersColors={roomUsersColors} onlineUsers={onlineUsers}
               className="room__users users" editableUsersID={editableUsersID} connector={connector}/>
        </div>
    );
};

export default Room;