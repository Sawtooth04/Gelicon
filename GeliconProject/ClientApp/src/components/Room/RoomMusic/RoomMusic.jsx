import React, {useEffect, useState} from 'react';
import {Route, Routes, NavLink} from "react-router-dom";
import MusicSearch from "./MusicSearch/MusicSearch";
import MusicList from "./MusicList/MusicList";
import MusicPlayer from "./MusicPlayer/MusicPlayer";
import PlaylistList from "./PlaylistList/PlaylistList";

const RoomMusic = ({connector, ...props}) => {
    const [roomMusicList, setRoomMusicList] = useState([]);
    const [playlists, setPlaylists] = useState([]);
    const [currentAudioInfo, setCurrentAudioInfo] = useState(null);
    const [needToDisplayMusicPlayer, setNeedToDisplayMusicPlayer] = useState(false);
    const [needToDisplayLoadingScreen, setNeedToDisplayLoadingScreen] = useState(true);

    useEffect(() => {
        setNeedToDisplayMusicPlayer(roomMusicList.length > 0);
    }, [roomMusicList]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetRoomMusicList", receiveRoomMusicList);
            connector.addEventHandler("SetRoomPlaylists", receiveRoomPlaylists);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetRoomMusicList", receiveRoomMusicList);
            connector.removeEventHandler("SetRoomPlaylists", receiveRoomPlaylists);
        }

        //Handlers
        async function receiveRoomMusicList(data) {
            setRoomMusicList(await connector.musicRepository.getMusicArrayFromApi(data));
        }

        async function receiveRoomPlaylists(data) {
            setPlaylists(data);
        }

        if (connector.connected)
            addEventHandlers();
        return () => removeEventHandlers();
    }, [connector, connector.connected, roomMusicList, playlists]);

    useEffect(() => {
        async function getRoomMusicList() {
            await connector.getRoomMusicList();
        }

        async function getRoomPlaylists() {
            await connector.getPlaylists();
        }

        if (connector.connected) {
            void getRoomMusicList();
            void getRoomPlaylists();
        }
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

    async function addPlaylist(playlist) {
        await connector.addPlaylistToRoom(playlist);
    }

    async function deletePlaylist(playlist) {
        await connector.deletePlaylist(playlist);
    }

    return (
        <div className={"room__room-music room-music"}>
            {!needToDisplayLoadingScreen ? null :
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
                <NavLink className="room-music__navbar__link" to="music-playlist-list"> Playlists </NavLink>
            </div>
            <div className={`room-music__routes${!needToDisplayMusicPlayer ? ' room-music__routes_max-size' : ''}`}>
                <Routes>
                    <Route exact path="music-list" element={
                        <MusicList musicList={roomMusicList} current={currentAudioInfo} setRoomMusic={setRoomMusic}
                           onDelete={deleteRoomMusic}/>
                    }/>
                    <Route exact path="music-playlist-list" element={
                        <PlaylistList playlists={playlists} current={currentAudioInfo} addCallback={addPlaylist}
                            deleteCallback={deletePlaylist}/>
                    }/>
                    <Route path="*" element={
                        <MusicSearch connector={connector} addMusicCallback={addMusic}
                            setNeedToDisplayLoadingScreen={setNeedToDisplayLoadingScreen}/>
                    }/>
                </Routes>
            </div>
            {
                needToDisplayMusicPlayer ?
                    <MusicPlayer connector={connector} setCurrentAudioInfoCallback={setCurrentAudioInfo}/> : null
            }
        </div>
    );
};

export default RoomMusic;