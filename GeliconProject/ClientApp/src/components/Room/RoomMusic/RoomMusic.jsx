import React, {useEffect, useState} from 'react';
import {Route, Routes, NavLink} from "react-router-dom";
import MusicSearch from "./MusicSearch/MusicSearch";
import MusicList from "./MusicList/MusicList";
import MusicPlayer from "./MusicPlayer/MusicPlayer";

const RoomMusic = ({connector, ...props}) => {
    const [roomMusicList, setRoomMusicList] = useState([]);
    const [currentAudioInfo, setCurrentAudioInfo] = useState(null);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetRoomMusicList", receiveRoomMusicList);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetRoomMusicList", receiveRoomMusicList);
        }

        //Handlers
        async function receiveRoomMusicList(data) {
            setRoomMusicList(await connector.musicRepository.getMusicArrayFromApi(data));
            removeEventHandlers();
        }

        if (connector.connected)
            addEventHandlers();
    }, [connector, connector.connected, roomMusicList]);

    useEffect(() => {
        async function getRoomMusicList() {
            await connector.getRoomMusicList();
        }

        if (connector.connected)
            void getRoomMusicList();
    }, [connector, connector.connected]);

    async function addMusic(musicID) {
        await connector.addMusicToRoom(musicID);
    }

    async function setRoomMusic(music) {
        await connector.setRoomMusic(music.id);
    }

    async function deleteRoomMusic(music) {
        await connector.deleteRoomMusic(music.id);
    }

    return (
        <div className={"room__room-music room-music"}>
            {(currentAudioInfo != null) ? null :
                <div className={"room-music__loading-screen loading-screen"}>
                    <div className={"loading-screen__animation-logo animation-logo"}>
                        <div className={"animation-logo__circle"}/>
                    </div>
                    <p className={"loading-screen__description"}>
                        Looking for an available server
                    </p>
                </div>
            }
            <div className="room-music__navbar">
                <NavLink className="room-music__navbar__link" to="music-search"> Search </NavLink>
                <NavLink className="room-music__navbar__link" to="music-list"> List </NavLink>
            </div>
            <div className={"room-music__routes"}>
                <Routes>
                    <Route exact path="music-list" element={
                        <MusicList musicList={roomMusicList} current={currentAudioInfo} setRoomMusic={setRoomMusic}
                           onDelete={deleteRoomMusic}/>
                    }/>
                    <Route path="*" element={<MusicSearch connector={connector} addMusicCallback={addMusic}/>}/>
                </Routes>
            </div>
            <MusicPlayer connector={connector} setCurrentAudioInfoCallback={setCurrentAudioInfo}/>
        </div>
    );
};

export default RoomMusic;