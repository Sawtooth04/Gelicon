import React from 'react';

const AddPlaylistLabel = ({onClick}) => {
    return (
        <div className={'playlists__playlists-item playlists-item add-playlist-label'} onClick={onClick}>
            <img src="/source/images/add-playlist.png"/>
        </div>
    );
};

export default AddPlaylistLabel;