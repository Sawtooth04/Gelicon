import React from 'react';
import Button from "../../../../UI/Button/Button";

const MusicListItem = ({item, current, onItemClick, onDelete, showPlaylists, onShowPlaylistsClick}) => {
    function onClick() {
        onItemClick(item);
    }

    function onDeleteClick() {
        onDelete(item);
    }

    function onShowPlaylists() {
        onShowPlaylistsClick(item);
    }

    return (
        <div className={`music-list__music-list-item music-list-item ${(current) ? 'music-list-item_current' : ''}`} onClick={onClick}>
            <div className={"music-list-item__description"}>
                <p> {item.title} </p>
            </div>
            <div className={"music-list-item__controls"}>
                <Button onClick={onDeleteClick} src={"/source/images/delete-music.png"}/>
                {showPlaylists ? <Button onClick={onShowPlaylists} src={"/source/images/playlist.png"}/> : null}
            </div>
        </div>
    );
};

export default MusicListItem;