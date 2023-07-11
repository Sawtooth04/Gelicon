import React, {useEffect, useState} from 'react';
import RoomsLabelsColorsHandler from "../../../Utils/RoomsLabelsColors";
import {useNavigate} from "react-router-dom";

const RoomLabel = (props) => {
    const [borderColor, setBorderColor] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        setBorderColor(RoomsLabelsColorsHandler.getRandomBorderColor());
    }, []);

    function joinRoom() {
        navigate(`/room/${props.room.roomID}/music-search`);
    }

    return (
        <div className={`rooms__rooms-label rooms-label rooms-label_${borderColor}`} onClick={joinRoom}>
            <h6 className="rooms-label__name"> {props.room.name} </h6>
            <p className="rooms-label__owner"> {props.room.owner.name} </p>
        </div>
    );
};

export default RoomLabel;