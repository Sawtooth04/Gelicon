import React, {useEffect, useState} from 'react';
import {Route, Routes, Link} from "react-router-dom";
import MusicSearch from "./MusicSearch/MusicSearch";
import MusicList from "./MusicList/MusicList";

const RoomMusic = ({connector, ...props}) => {
    const [roomMusicList, setRoomMusicList] = useState([]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetRoomMusic", receiveRoomMusicList);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetRoomMusic", receiveRoomMusicList);
        }

        async function getRoomMusic() {
            await connector.getRoomMusic();
        }

        //Handlers
        async function receiveRoomMusicList(data) {
            setRoomMusicList(await connector.musicRepository.getMusicArrayFromApi(data));
            removeEventHandlers();
        }

        if (connector.connected) {
            addEventHandlers();
            void getRoomMusic();
        }
    }, [connector, connector.connected]);

    async function addMusic(musicID) {
        await connector.addMusicToRoom(musicID);
    }

    return (
        <div className={"room__room-music room-music"}>
            <div className="room-music__navbar">
                <Link to="music-search"> Search </Link>
                <Link to="music-list"> Search </Link>
            </div>
            <Routes>
                <Route exact path="music-list" element={<MusicList musicList={roomMusicList}/>}/>
                <Route path="*" element={<MusicSearch connector={connector} addMusicCallback={addMusic}/>}/>
            </Routes>
        </div>
    );
};

export default RoomMusic;