import React from 'react';

const PlaylistMusicDialogLabel = ({playlist, addToPlaylistCallback, isTarget}) => {
    function addToPlaylist() {
        addToPlaylistCallback(playlist);
    }

    return (
        <div className={`playlist-music-dialog-label${isTarget ? ' playlist-music-dialog-label_target' : ''}`} onClick={addToPlaylist}>
            <div className={"playlist-music-dialog-label__description"}>
                <p> {playlist.name} </p>
            </div>
        </div>
    );
};

export default PlaylistMusicDialogLabel;