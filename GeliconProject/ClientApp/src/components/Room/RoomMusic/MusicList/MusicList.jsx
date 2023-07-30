import React from 'react';
import MusicListItem from "./MusicListItem/MusicListItem";
import ScrollView from "../../../UI/ScrollView/ScrollView";
import LoadingSpinner from "../../../UI/LoadingSpinner/LoadingSpinner";

const MusicList = ({musicList, current, setRoomMusic, onDelete, onNextCallback, onPrevCallback, loadingState}) => {
    function onItemClick(item) {
        setRoomMusic(item);
    }

    function onDeleteClick(item) {
        onDelete(item);
    }

    function onNext() {
        onNextCallback();
    }

    function onPrev() {
        onPrevCallback();
    }

    return (
        <div className={"room-music__music-list music-list"}>
            {(loadingState) ? <LoadingSpinner/> : null}
            <ScrollView threshold={0.85} nextCallback={onNext} prevCallback={onPrev} loadingState={loadingState} elements=
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
            />
        </div>
    );
};

export default MusicList;