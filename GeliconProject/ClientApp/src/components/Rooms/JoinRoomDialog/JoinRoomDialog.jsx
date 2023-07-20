import React, {useState} from 'react';
import {useNavigate} from "react-router-dom";

const JoinRoomDialog = ({cancelCallback}) => {
    const [token, setToken] = useState('');
    const navigate = useNavigate();

    async function join() {
        let response = await fetch('/rooms/join-room?' + new URLSearchParams({ token: token, }), {method: "get"});

        if (response.ok) {
            cancelCallback();
            navigate(`/room/${await response.json()}/music-search`);
        }
    }

    function onTokenChange(event) {
        setToken(event.target.value);
    }

    return (
        <div className={"add-room-dialog"}>
            <div className={"add-room-dialog__content"}>
                <div className={"add-room-dialog__content__header"}>
                    <p className={"add-room-dialog__content__header__title"}> Join to room </p>
                </div>
                <div className={"add-room-dialog__content__body"}>
                    <p className="add-room-dialog__content__body__title"> Enter room token: </p>
                    <input type="text" className="add-room-dialog__content__body__input" value={token}
                           onChange={onTokenChange}/>
                </div>
                <div className={"add-room-dialog__content__footer"}>
                    <button className={"add-room-dialog__content__footer__button"} onClick={cancelCallback}>
                        Cancel
                    </button>
                    <button className={"add-room-dialog__content__footer__button main-button"} onClick={join}>
                        Join
                    </button>
                </div>
            </div>
        </div>
    );
};

export default JoinRoomDialog;