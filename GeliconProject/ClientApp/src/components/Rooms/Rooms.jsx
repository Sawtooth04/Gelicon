import React, {useEffect, useState} from 'react';
import RoomLabel from "./RoomLabel/RoomLabel";
import AddRoomLabel from "./AddRoomLabel/AddRoomLabel";
import AddRoomDialog from "./AddRoomDialog/AddRoomDialog";
import {useNavigate} from "react-router-dom";
import JoinRoomLabel from "./JoinRoomLabel/JoinRoomLabel";
import JoinRoomDialog from "./JoinRoomDialog/JoinRoomDialog";

const Rooms = ({onMount}) => {
    const [rooms, setRooms] = useState([]);
    const [addRoomDialogOpened, setAddRoomDialogOpened] = useState(false);
    const [joinRoomDialogOpened, setJoinRoomDialogOpened] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {

        const roomsInit = async () => {
            let data = await getRooms();
            setRooms(data);
        };

        onMount();
        void roomsInit();
    }, []);

    async function getRooms() {
        let response = await fetch('rooms/get-rooms',{method: "get"});
        return await response.json();
    }

    function roomLabelClickCallback(room) {
        navigate(`/room/${room.roomID}/music-search`);
    }

    function addRoomLabelClickCallback() {
        setAddRoomDialogOpened(true);
    }

    function cancelAddRoomCallback() {
        setAddRoomDialogOpened(false);
    }

    function joinRoomLabelClickCallback() {
        setJoinRoomDialogOpened(true);
    }

    function cancelJoinRoomCallback() {
        setJoinRoomDialogOpened(false);
    }

    return (
        <div className="rooms">
            <AddRoomLabel onClick={addRoomLabelClickCallback}/>
            <JoinRoomLabel onClick={joinRoomLabelClickCallback}/>
            {
                rooms.map((room) =>
                    <RoomLabel room={room} key={room.roomID} onClick={roomLabelClickCallback}/>
                )
            }
            {(addRoomDialogOpened) ?
                <AddRoomDialog cancelCallback={cancelAddRoomCallback}/> :
                null
            }
            {(joinRoomDialogOpened) ?
                <JoinRoomDialog cancelCallback={cancelJoinRoomCallback}/> :
                null
            }
        </div>
    );
};

export default Rooms;