import React, {useState} from 'react';
import {useNavigate} from "react-router-dom";

const RoomAuthorizationDialog = ({room, cancelCallback}) => {
    const [password, setPassword] = useState('');
    const [isIncorrect, setIsIncorrect] = useState(false);
    const navigate = useNavigate();

    async function joinRoom() {
        setIsIncorrect(false);
        let response = await fetch("/room/room-authorization", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify([room.roomID, password])
        });
        if (response.ok)
            navigate(`/room/${room.roomID}/music-search`);
        else
            setIsIncorrect(true);
    }

    function onPasswordChange(event) {
        setPassword(event.target.value);
    }

    return (
        <div className={`room-authorization-dialog${isIncorrect ? ' room-authorization-dialog_incorrect' : ''}`}>
            <div className={"room-authorization-dialog__content"}>
                <div className={"room-authorization-dialog__content__header"}>
                    <p className={"room-authorization-dialog__content__header__title"}> {room.name} </p>
                </div>
                <div className={"room-authorization-dialog__content__body"}>
                    <p className="room-authorization-dialog__content__body__title"> Enter room password: </p>
                    <input type="password" className="room-authorization-dialog__content__body__input" value={password}
                       onChange={onPasswordChange}/>
                </div>
                <div className={"room-authorization-dialog__content__footer"}>
                    <button className={"room-authorization-dialog__content__footer__button"} onClick={cancelCallback}>
                        Cancel
                    </button>
                    <button className={"room-authorization-dialog__content__footer__button main-button"} onClick={joinRoom}>
                        Join
                    </button>
                </div>
            </div>
        </div>
    );
};

export default RoomAuthorizationDialog;