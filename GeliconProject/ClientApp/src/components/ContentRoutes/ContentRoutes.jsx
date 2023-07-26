import React, {useState} from 'react';
import Sidebar from "../Sidebar/Sidebar";
import Header from "../Header/Header";
import {Route, Routes, Navigate} from "react-router-dom";
import Rooms from "../Rooms/Rooms";
import Room from "../Room/Room";

const ContentRoutes = ({onNavigate}) => {
    const [isInRoom, setIsInRoom] = useState(false);
    const [showRoomJoinToken, setShowRoomJoinToken] = useState(false);

    function onRoomMount() {
        setIsInRoom(true);
        onNavigate();
    }

    function onRoomsMount() {
        setIsInRoom(false);
        onNavigate();
    }

    return (
        <div className="wrapper">
            <Header/>
            <div className="main">
                <Sidebar showRoomButtons={isInRoom} setShowRoomJoinToken={() => setShowRoomJoinToken(true)}/>
                <div className="content">
                    <Routes>
                        <Route exact path="/room/:roomID/*" element={
                            <Room onMount={onRoomMount} showRoomJoinToken={showRoomJoinToken}
                                  closeRoomJoinTokenDialog={() => setShowRoomJoinToken(false)}/>
                        }/>
                        <Route path="*" element={<Rooms onMount={onRoomsMount}/>}/>
                    </Routes>
                </div>
            </div>
        </div>
    );
};

export default ContentRoutes;