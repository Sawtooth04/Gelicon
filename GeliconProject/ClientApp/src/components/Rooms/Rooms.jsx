import React, {useEffect, useState} from 'react';
import RoomLabel from "./RoomLabel/RoomLabel";
import AddRoomLabel from "./AddRoomLabel/AddRoomLabel";
import AddRoomDialog from "./AddRoomDialog/AddRoomDialog";
import {useNavigate} from "react-router-dom";

const Rooms = () => {
    const [rooms, setRooms] = useState([]);
    const [addRoomDialogOpened, setAddRoomDialogOpened] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const roomsInit = async () => {
            let data = await getRooms();
            setRooms(data);
        };
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

    return (
        <div className="rooms">
            <AddRoomLabel onClick={addRoomLabelClickCallback}/>
            {
                rooms.map((room) =>
                    <RoomLabel room={room} key={room.roomID} onClick={roomLabelClickCallback}/>
                )
            }
            {(addRoomDialogOpened) ?
                <AddRoomDialog cancelCallback={cancelAddRoomCallback}/> :
                null
            }
        </div>
    );
};

export default Rooms;