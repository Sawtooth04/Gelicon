import React from 'react';
import MusicSearchListItem from "./MusicSearchListItem/MusicSearchListItem";

const MusicSearchList = ({searchResults, addMusicCallback}) => {
    return (
        <div className={"room-music__music-search-list music-search-list"}>
            {searchResults.map((searchResult) =>
                <MusicSearchListItem item={searchResult} key={searchResult.id} addMusicCallback={addMusicCallback}/>
            )}
        </div>
    );
};

export default MusicSearchList;