import React, {useEffect, useState} from 'react';
import RoomLabel from "./RoomLabel/RoomLabel";

const Rooms = () => {
    const [rooms, setRooms] = useState([]);

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

    return (
        <div className="rooms">
            {
                rooms.map((room) =>
                    <RoomLabel room={room} key={room.roomID}/>
                )
            }
        </div>
    );
};

export default Rooms;