import React from 'react';
import Button from "../../../../../UI/Button/Button";

const MusicSearchListItem = ({item, addMusicCallback}) => {
    async function onAddMusicClick() {
        await addMusicCallback(item.id);
    }

    return (
        <div className={"music-search-list__music-search-list-item music-search-list-item"}>
            <div className={"music-search-list-item__description"}>
                <p> {item.title} </p>
            </div>
            <div className={"music-search-list-item__controls"}>
                <Button onClick={onAddMusicClick} src={"/source/images/add-music.png"}/>
            </div>
        </div>
    );
};

export default MusicSearchListItem;