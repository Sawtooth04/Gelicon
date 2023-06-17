import React, {useState, useEffect} from 'react';
import MusicListItem from "./MusicListItem/MusicListItem";

const MusicList = ({musicList}) => {
    return (
        <div className={"room-music__music-list music-list"}>
            {musicList.map((music) =>
                <MusicListItem item={music.data} key={music.data.id}/>
            )}
        </div>
    );
};

export default MusicList;