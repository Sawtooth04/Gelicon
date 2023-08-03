import React, {useState, useEffect} from 'react';
import MusicList from "../../MusicList/MusicList";

const EditPlaylistDialog = ({playlist, editCallback, cancelCallback, onClick, onDelete, addEventHandler,
        removeEventHandler, getMusicList, getMusicFromApi}) => {
    const countByPage = 10;
    const maxCount = 50;
    const [musicList, setMusicList] = useState([]);
    const [scrollPage, setScrollPage] = useState(0);
    const [isLoadingMusicList, setIsLoadingMusicList] = useState(true);
    const [name, setName] = useState(playlist.name);

    useEffect(() => {
        function addEventHandlers() {
            addEventHandler("AppendPlaylistMusicList", appendMusicList);
            addEventHandler("AppendBeforePlaylistMusicList", appendBeforeMusicList);
        }

        function removeEventHandlers() {
            removeEventHandler("AppendPlaylistMusicList", appendMusicList);
            removeEventHandler("AppendBeforePlaylistMusicList", appendBeforeMusicList);
        }

        //Handlers
        async function appendMusicList(data) {
            let music = await getMusicFromApi(data);
            if (music.length > 0) {
                setMusicList([...musicList, ...music]);
                setScrollPage(scrollPage + 1);
            }
            setIsLoadingMusicList(false);
            if (musicList.length > maxCount)
                setMusicList(musicList.slice(musicList.length - maxCount, musicList.length));
        }

        async function appendBeforeMusicList(data) {
            let music = await getMusicFromApi(data);
            if (music.length > 0) {
                setMusicList([...music, ...musicList]);
                setScrollPage(scrollPage - 1);
            }
            setIsLoadingMusicList(false);
            if (musicList.length > maxCount)
                setMusicList(musicList.slice(0, musicList.length - (musicList.length - maxCount)));
        }

        addEventHandlers();
        return () => removeEventHandlers();
    }, [musicList]);

    useEffect(() => {
        void getMusicList(playlist.roomPlaylistID, 0, countByPage, true);
    }, []);

    async function editPlaylist() {
        editCallback(playlist, name);
        cancelCallback();
    }

    function onNameChange(event) {
        setName(event.target.value);
    }

    function onNext() {
        let start = scrollPage * countByPage;
        setIsLoadingMusicList(true);
        void getMusicList(playlist.roomPlaylistID, start, countByPage, true);
    }

    function onPrev() {
        if (scrollPage > maxCount / countByPage && scrollPage > 1) {
            let start = scrollPage - maxCount / countByPage - 1;
            setIsLoadingMusicList(true);
            void getMusicList(playlist.roomPlaylistID, start, countByPage, false);
        }
    }

    function onDeleteCallback(item) {
        onDelete(playlist.roomPlaylistID, item.id);
    }

    function onPlaylistMusicClick(music) {
        onClick(playlist, music);
    }

    return (
        <div className={"edit-playlist-dialog"}>
            <div className={"edit-playlist-dialog__content"}>
                <div className={"edit-playlist-dialog__content__header"}>
                    <p className={"edit-playlist-dialog__content__header__title"}> Edit playlist </p>
                </div>
                <div className={"edit-playlist-dialog__content__body"}>
                    <p className="edit-playlist-dialog__content__body__title"> Playlist name: </p>
                    <input type="text" className="edit-playlist-dialog__content__body__input" value={name}
                           onChange={onNameChange}/>
                </div>
                <MusicList musicList={musicList} current={null} onClick={onPlaylistMusicClick} onDelete={onDeleteCallback}
                    onNextCallback={onNext} onPrevCallback={onPrev} loadingState={isLoadingMusicList} showPlaylists={false}/>
                <div className={"edit-playlist-dialog__content__footer"}>
                    <button className={"edit-playlist-dialog__content__footer__button"} onClick={cancelCallback}>
                        Cancel
                    </button>
                    <button className={"edit-playlist-dialog__content__footer__button main-button"} onClick={editPlaylist}>
                        Confirm
                    </button>
                </div>
            </div>
        </div>
    );
};

export default EditPlaylistDialog;