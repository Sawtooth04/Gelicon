import React, {useState} from 'react';
import PlaylistListItem from "./PlaylistListItem/PlaylistListItem";
import AddPlaylistLabel from "./AddPlaylistLabel/AddPlaylistLabel";
import AddPlaylistDialog from "./AddPlaylistDialog/AddPlaylistDialog";
import EditPlaylistDialog from "./EditPlaylistDialog/EditPlaylistDialog";

const PlaylistList = ({playlists, current, addCallback, deleteCallback, editCallback, onClick, onDelete, addEventHandler,
        removeEventHandler, getMusicList, getMusicFromApi}) => {
    const [addPlaylistDialogOpened, setAddPlaylistDialogOpened] = useState(false);
    const [editPlaylistDialogOpened, setEditPlaylistDialogOpened] = useState(false);
    const [currentEditPlaylist, setCurrentEditPlaylist] = useState(null);

    function onItemClick(item) {
        setEditPlaylistDialogOpened(true);
        setCurrentEditPlaylist(item);
    }

    async function setChanges(playlist, name) {
        editCallback(playlist, name);
        setEditPlaylistDialogOpened(false);
    }

    function onDeleteClick(item) {
        deleteCallback(item);
    }

    function onAddPlaylistLabelClick() {
        setAddPlaylistDialogOpened(true);
    }

    function onCancelAddPlaylistDialog() {
        setAddPlaylistDialogOpened(false);
    }

    function onCancelEditPlaylistDialog() {
        setEditPlaylistDialogOpened(false);
    }

    return (
        <div className={"room-music__playlists playlists"}>
            {editPlaylistDialogOpened ?
                <EditPlaylistDialog playlist={currentEditPlaylist} cancelCallback={onCancelEditPlaylistDialog}
                    editCallback={setChanges} onClick={onClick} onDelete={onDelete} addEventHandler={addEventHandler}
                    removeEventHandler={removeEventHandler} getMusicList={getMusicList} getMusicFromApi={getMusicFromApi}/>
                : null
            }
            {addPlaylistDialogOpened ?
                <AddPlaylistDialog addCallback={addCallback} cancelCallback={onCancelAddPlaylistDialog}/>
                : null
            }
            <AddPlaylistLabel onClick={onAddPlaylistLabelClick}/>
            {
                (current != null) ?
                    playlists.map((playlist) =>
                        <PlaylistListItem item={playlist} key={playlist.roomPlaylistID} onItemClick={onItemClick}
                            onDelete={onDeleteClick} current={playlist.roomPlaylistID === current.roomPlaylistID}/>
                    ) :
                    playlists.map((playlist) =>
                        <PlaylistListItem item={playlist} key={playlist.roomPlaylistID} onItemClick={onItemClick}
                            onDelete={onDeleteClick} current={false}/>
                    )
            }
        </div>
    );
};

export default PlaylistList;