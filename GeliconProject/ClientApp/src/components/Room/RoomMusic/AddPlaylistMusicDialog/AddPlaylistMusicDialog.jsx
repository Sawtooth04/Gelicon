import React, {useState} from 'react';
import PlaylistMusicDialogLabel from "../PlaylistMusicDialogLabel/PlaylistMusicDialogLabel";

const AddPlaylistMusicDialog = ({music, cancelCallback, playlists, addToPlaylistCallback, targeted}) => {
    const [targetPlaylists, setTargetPlaylists] = useState([targeted]);

    function addToPlaylist(playlist) {
        let tempPlaylists = [...targetPlaylists];

        if (tempPlaylists.includes(playlist))
            tempPlaylists.splice(tempPlaylists.indexOf(playlist), 1);
        else
            tempPlaylists.push(playlist);
        setTargetPlaylists(tempPlaylists);
    }

    function addToAllTargetPlaylists() {
        targetPlaylists.forEach((playlist) => {
            addToPlaylistCallback(music, playlist);
        });
        cancelCallback();
    }

    return (
        <div className="add-playlist-music-dialog">
            <div className={"add-playlist-music-dialog__content"}>
                <div className={"add-playlist-music-dialog__content__header"}>
                    <p className={"add-playlist-music-dialog__content__header__title"}> Add music to playlist </p>
                </div>
                <div className={"add-playlist-music-dialog__content__body"}>
                    {playlists.map((playlist) => <PlaylistMusicDialogLabel key={playlist.roomPlaylistID} playlist={playlist}
                        addToPlaylistCallback={addToPlaylist} isTarget={targetPlaylists.includes(playlist)}/>)}
                </div>
                <div className={"add-playlist-music-dialog__content__footer"}>
                    <button className={"add-playlist-music-dialog__content__footer__button main-button"} onClick={addToAllTargetPlaylists}>
                        Confirm
                    </button>
                </div>
            </div>
        </div>
    );
};

export default AddPlaylistMusicDialog;