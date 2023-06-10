import React from 'react';

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
                <button className={"music-search-list-item__controls__add-music-button add-music-button"} onClick={onAddMusicClick}>
                    <img src={"/source/images/add-music.png"}/>
                </button>
            </div>
        </div>
    );
};

export default MusicSearchListItem;