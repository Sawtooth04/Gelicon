import React, {useState, useEffect} from 'react';
import TextArea from "../../../UI/TextArea/TextArea";
import MusicSearchList from "./MusicSearchList/MusicSearchList";
import Button from "../../../UI/Button/Button";

const MusicSearch = ({connector, addMusicCallback, setNeedToDisplayLoadingScreen}) => {
    const [query, setQuery] = useState("");
    const [searchResults, setSearchResults] = useState([]);

    useEffect(() => {
        async function initSearchResults() {
            let response = await connector.musicRepository.executeQuery(
                connector.musicRepository.getTrendingTracks.bind(connector.musicRepository),
                new Map([])
            );
            setSearchResults((await response.json()).data);
            setNeedToDisplayLoadingScreen(false);
        }

        if (connector.connected)
            void initSearchResults();
    }, [connector, connector.connected]);

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
                <Button onClick={search} src={"/source/images/search.png"}/>
            </div>
            <MusicSearchList searchResults={searchResults} addMusicCallback={addMusicCallback}/>
        </div>
    );
};

export default MusicSearch;