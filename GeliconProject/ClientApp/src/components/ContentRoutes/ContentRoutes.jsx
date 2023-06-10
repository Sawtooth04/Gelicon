import React from 'react';
import Sidebar from "../Sidebar/Sidebar";
import Header from "../Header/Header";
import {Route, Routes} from "react-router-dom";
import Rooms from "../Rooms/Rooms";
import Room from "../Room/Room";

const ContentRoutes = () => {
    return (
        <div className="wrapper">
            <Sidebar/>
            <div className="main">
                <Header/>
                <div className="content">
                    <Routes>
                        <Route exact path="/room/:roomID/*" element={<Room/>}/>
                        <Route path="*" element={<Rooms/>}/>
                    </Routes>
                </div>
            </div>
        </div>
    );
};

export default ContentRoutes;