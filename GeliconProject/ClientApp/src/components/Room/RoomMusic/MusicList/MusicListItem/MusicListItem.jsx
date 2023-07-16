import React from 'react';
import Button from "../../../../UI/Button/Button";

const MusicListItem = ({item, current, onItemClick, onDelete}) => {
    function onClick() {
        onItemClick(item);
    }

    function onDeleteClick() {
        onDelete(item);
    }

    return (
        <div className={`music-list__music-list-item music-list-item ${(current) ? 'music-list-item_current' : ''}`} onClick={onClick}>
            <div className={"music-list-item__description"}>
                <p> {item.title} </p>
            </div>
            <div className={"music-list-item__controls"}>
                <Button onClick={onDeleteClick} src={"/source/images/delete-music.png"}/>
            </div>
        </div>
    );
};

export default MusicListItem;