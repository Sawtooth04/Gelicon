import React, {useEffect, useState} from 'react';
import {useParams} from "react-router-dom";
import RoomChat from "./RoomChat/RoomChat";
import HubConnector from "../../Utils/HubConnector";
import RoomUsersList from "./RoomUsersList/RoomUsersList";
import RoomMusic from "./RoomMusic/RoomMusic";
import RoomMusicPlayer from "./RoomMusicPlayer/RoomMusicPlayer";

const Room = () => {
    const [connector, setConnector] = useState(new HubConnector());
    const {roomID} = useParams();
    const [users, setUsers] = useState([]);
    const [roomUsersColors, setRoomUsersColors] = useState([]);
    const [colors, setColors] = useState([]);

    useEffect(() => {
        return () => connector.disconnect();
    }, []);

    useEffect(() => {
        async function getRoom() {
            return await fetch("/room/join", {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(roomID)
            });
        }

        async function getColors() {
            return await fetch("/color/get-colors", {
                method: "GET",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
            });
        }

        function setStates(room) {
            setUsers(room.users);
            setRoomUsersColors(room.roomUsersColors);
        }

        const init = async () => {
            setColors(await (await getColors()).json());
            setStates(await (await getRoom()).json());
        };

        connector.setConnectionToHub(roomID).then(() => void init());
    }, [connector]);

    return (
        <div className="room">
            <RoomMusic connector={connector}/>
            <RoomChat connector={connector} roomID={roomID} roomUsersColors={roomUsersColors}/>
            <RoomUsersList users={users} roomUsersColors={roomUsersColors} className="room__users users"/>
            <RoomMusicPlayer connector={connector}/>
        </div>
    );
};

export default Room;