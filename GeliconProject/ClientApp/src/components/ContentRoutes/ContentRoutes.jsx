import React, {useState} from 'react';
import Sidebar from "../Sidebar/Sidebar";
import Header from "../Header/Header";
import {Route, Routes} from "react-router-dom";
import Rooms from "../Rooms/Rooms";
import Room from "../Room/Room";

const ContentRoutes = () => {
    const [isInRoom, setIsInRoom] = useState(false);
    const [showRoomJoinToken, setShowRoomJoinToken] = useState(false);

    return (
        <div className="wrapper">
            <Sidebar showRoomButtons={isInRoom} setShowRoomJoinToken={() => setShowRoomJoinToken(true)}/>
            <div className="main">
                <Header/>
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