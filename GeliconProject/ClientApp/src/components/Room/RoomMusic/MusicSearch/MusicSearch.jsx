import React, {useState} from 'react';
import TextArea from "../../../UI/TextArea/TextArea";
import MusicSearchList from "./MusicSearchList/MusicSearchList";

const MusicSearch = ({connector, addMusicCallback}) => {
    const [query, setQuery] = useState("");
    const [searchResults, setSearchResults] = useState([]);

    function onInput(query) {
        setQuery(query);
    }

    async function search() {
        let response = await connector.musicRepository.executeQuery(
            connector.musicRepository.searchMusic.bind(connector.musicRepository),
            new Map([['query', query]])
        );
        setSearchResults((await response.json()).data);
    }

    return (
        <div className={"room-music__music-search music-search"}>
            <div className={"music-search__controls music-search-controls"}>
                <TextArea className="music-search-controls__query-input" value={query} inputCallback={onInput}
                          maxHeightProp={document.documentElement.offsetHeight * 0.05}/>
                <button className={"music-search-controls__button"} onClick={search}> Search </button>
            </div>
            <MusicSearchList searchResults={searchResults} addMusicCallback={addMusicCallback}/>
        </div>
    );
};

export default MusicSearch;