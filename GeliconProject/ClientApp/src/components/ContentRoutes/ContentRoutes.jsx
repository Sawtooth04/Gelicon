import React, {useState, useEffect} from 'react';
import Sidebar from "../Sidebar/Sidebar";
import Header from "../Header/Header";
import {Route, Routes, useNavigate} from "react-router-dom";
import Rooms from "../Rooms/Rooms";
import Room from "../Room/Room";

const ContentRoutes = () => {
    const [isInRoom, setIsInRoom] = useState(false);
    const [showRoomJoinToken, setShowRoomJoinToken] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        async function authorizationCheck() {
            let response = await fetch('/authorization/authorization-check', {
                method: 'get'
            });
            if (!response.ok)
                navigate('/login');
        }

        void authorizationCheck();
    });

    return (
        <div className="wrapper">
            <Header/>
            <div className="main">
                <Sidebar showRoomButtons={isInRoom} setShowRoomJoinToken={() => setShowRoomJoinToken(true)}/>
                <div className="content">
                    <Routes>
                        <Route exact path="/room/:roomID/*" element={
                            <Room onMount={() => setIsInRoom(true)} showRoomJoinToken={showRoomJoinToken}
                                closeRoomJoinTokenDialog={() => setShowRoomJoinToken(false)}/>
                        }/>
                        <Route path="*" element={<Rooms onMount={() => setIsInRoom(false)}/>}/>
                    </Routes>
                </div>
            </div>
        </div>
    );
};

export default ContentRoutes;