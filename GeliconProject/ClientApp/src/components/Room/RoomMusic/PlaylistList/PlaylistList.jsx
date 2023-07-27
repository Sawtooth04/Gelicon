import React, {useState} from 'react';
import PlaylistListItem from "./PlaylistListItem/PlaylistListItem";
import AddPlaylistLabel from "./AddPlaylistLabel/AddPlaylistLabel";
import AddPlaylistDialog from "./AddPlaylistDialog/AddPlaylistDialog";

const PlaylistList = ({playlists, current, addCallback, deleteCallback}) => {
    const [addPlaylistDialogOpened, setAddPlaylistDialogOpened] = useState(false);

    function onItemClick(item) {

    }

    function onDeleteClick(item) {
        deleteCallback(item);
    }

    function onAddPlaylistLabelClick() {
        setAddPlaylistDialogOpened(true);
    }

    function onCancelPlaylistDialog() {
        setAddPlaylistDialogOpened(false);
    }

    return (
        <div className={"room-music__playlists playlists"}>
            {addPlaylistDialogOpened ?
                <AddPlaylistDialog addCallback={addCallback} cancelCallback={onCancelPlaylistDialog}/>
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