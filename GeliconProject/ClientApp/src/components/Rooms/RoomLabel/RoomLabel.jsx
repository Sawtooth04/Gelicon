import React, {useEffect, useState} from 'react';
import RoomsLabelsColorsHandler from "../../../Utils/RoomsLabelsColors";

const RoomLabel = ({room, onClick}) => {
    const [borderColor, setBorderColor] = useState("");

    useEffect(() => {
        setBorderColor(RoomsLabelsColorsHandler.getRandomBorderColor());
    }, []);

    return (
        <div className={`rooms__rooms-label rooms-label rooms-label_${borderColor}`} onClick={() => onClick(room)}>
            <h6 className="rooms-label__name"> {room.name} </h6>
            <p className="rooms-label__owner"> {room.owner.name} </p>
        </div>
    );
};

export default RoomLabel;