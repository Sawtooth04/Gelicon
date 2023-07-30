import React, {useState} from 'react';

const EditPlaylistDialog = ({playlist, editCallback, cancelCallback}) => {
    const [name, setName] = useState(playlist.name);

    async function editPlaylist() {
        editCallback(playlist, name);
        cancelCallback();
    }

    function onNameChange(event) {
        setName(event.target.value);
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