import React, {useState, useEffect} from 'react';
import MusicListItem from "./MusicListItem/MusicListItem";

const MusicList = ({musicList, current, setRoomMusic, onDelete}) => {
    function onItemClick(item) {
        setRoomMusic(item);
    }

    function onDeleteClick(item) {
        onDelete(item);
    }

    return (
        <div className={"room-music__music-list music-list"}>
            {
                (current != null) ?
                musicList.map((music) =>
                    <MusicListItem item={music.data} key={music.data.id} current={music.data.id === current.id}
                        onItemClick={onItemClick} onDelete={onDeleteClick}/>
                ) :
                musicList.map((music) =>
                    <MusicListItem item={music.data} key={music.data.id} current={false}
                        onItemClick={onItemClick} onDelete={onDeleteClick}/>
                )
            }
        </div>
    );
};

export default MusicList;