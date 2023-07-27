import React from 'react';
import Button from "../../../../UI/Button/Button";

const PlaylistListItem = ({item, current, onItemClick, onDelete}) => {
    function onClick() {
        onItemClick(item);
    }

    function onDeleteClick() {
        onDelete(item)
    }

    return (
        <div className={`playlists__playlists-item playlists-item ${(current) ? 'playlists-item_current' : ''}`} onClick={onClick}>
            <div className={"playlists-item__description"}>
                <p> {item.name} </p>
            </div>
            <div className={"playlists-item__controls"}>
                <Button onClick={onDeleteClick} src={"/source/images/delete-music.png"}/>
            </div>
        </div>
    );
};

export default PlaylistListItem;