import React from 'react';
import {Route, Routes, Link} from "react-router-dom";
import MusicSearch from "./MusicSearch/MusicSearch";

const RoomMusic = ({connector, ...props}) => {
    async function addMusic(musicID) {
        await connector.addMusicToRoom(musicID);
    }

    return (
        <div className={"room__room-music room-music"}>
            <div className="room-music__navbar">
                <Link to="music-search"> Search </Link>
            </div>
            <Routes>
                <Route path="*" element={<MusicSearch connector={connector} addMusicCallback={addMusic}/>}/>
            </Routes>
        </div>
    );
};

export default RoomMusic;