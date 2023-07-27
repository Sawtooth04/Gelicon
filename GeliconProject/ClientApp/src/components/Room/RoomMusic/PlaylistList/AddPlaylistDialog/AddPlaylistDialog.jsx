import React, {useState} from 'react';

const AddPlaylistDialog = ({addCallback, cancelCallback}) => {
    const [name, setName] = useState('');

    async function addPlaylist() {
        addCallback({name: name});
        cancelCallback();
    }

    function onNameChange(event) {
        setName(event.target.value);
    }

    return (
        <div className={"add-playlist-dialog"}>
            <div className={"add-playlist-dialog__content"}>
                <div className={"add-playlist-dialog__content__header"}>
                    <p className={"add-playlist-dialog__content__header__title"}> Create new playlist </p>
                </div>
                <div className={"add-playlist-dialog__content__body"}>
                    <p className="add-playlist-dialog__content__body__title"> Enter playlist name: </p>
                    <input type="text" className="add-playlist-dialog__content__body__input" value={name}
                           onChange={onNameChange}/>
                </div>
                <div className={"add-playlist-dialog__content__footer"}>
                    <button className={"add-playlist-dialog__content__footer__button"} onClick={cancelCallback}>
                        Cancel
                    </button>
                    <button className={"add-playlist-dialog__content__footer__button main-button"} onClick={addPlaylist}>
                        Create
                    </button>
                </div>
            </div>
        </div>
    );
};

export default AddPlaylistDialog;