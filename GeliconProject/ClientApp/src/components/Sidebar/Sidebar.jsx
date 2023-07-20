import React, {useState} from 'react';
import Button from "../UI/Button/Button";
import {useNavigate} from "react-router-dom";

const Sidebar = ({showRoomButtons, setShowRoomJoinToken}) => {
    const navigate = useNavigate();

    function onHomeClick() {
        navigate("/rooms");
    }

    return (
        <div className="sidebar">
            {showRoomButtons ?
                <div className={'sidebar__room-buttons'}>
                    <Button className={'sidebar__button'} onClick={setShowRoomJoinToken} src={'/source/images/home.png'}/>
                </div> : null
            }
            <Button className={'sidebar__button'} onClick={onHomeClick} src={'/source/images/home.png'}/>
        </div>
    );
};

export default Sidebar;