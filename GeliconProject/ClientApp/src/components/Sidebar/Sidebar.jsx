import React, {useState} from 'react';
import Button from "../UI/Button/Button";
import {useNavigate} from "react-router-dom";

const Sidebar = ({showRoomButtons, setShowRoomJoinToken}) => {
    const navigate = useNavigate();

    function onHomeClick() {
        navigate("/rooms");
    }

    function onGithubClick() {
        window.open(
            'https://github.com/Sawtooth04/Gelicon',
            '_blank'
        );
    }

    return (
        <div className="sidebar">
            {showRoomButtons ?
                <div className={'sidebar__room-buttons'}>
                    <Button className={'sidebar__button'} onClick={setShowRoomJoinToken} src={'/source/images/connect.png'}/>
                </div> : null
            }
            <Button className={'sidebar__button'} onClick={onHomeClick} src={'/source/images/home.png'}/>
            <Button className={'sidebar__button'} onClick={onGithubClick} src={'/source/images/github.png'}/>
        </div>
    );
};

export default Sidebar;