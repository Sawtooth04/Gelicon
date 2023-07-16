import React, {useState, useEffect} from 'react';
import "@melloware/coloris/dist/coloris.css";
import Coloris from "@melloware/coloris";
import {useNavigate} from "react-router-dom";

const AddRoomDialog = ({cancelCallback}) => {
    const [name, setName] = useState('');
    const [defaultColor, setDefaultColor] = useState('#a7a7a7');
    const navigate = useNavigate();

    useEffect(() => {
        Coloris.init();
        Coloris({
            el: ".input-coloris",
            themeMode: 'dark',
            alpha: false,
            margin: 0,
            swatches: [
                '#FF0000',
                '#00FF00',
                '#0000FF',
                '#FF1493',
                '#FF8C00',
                '#FFDAB9',
                '#8A2BE2',
                '#20B2AA',
                '#1E90FF',
            ]
        });
    }, []);

    async function addRoom() {
        let response = await fetch("/room/add-room", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({name: name, defaultColor: defaultColor})
        });
        if (response.ok) {
            cancelCallback();
            navigate(`/room/${(await response.json()).roomID}/music-search`);
        }
    }

    function onNameChange(event) {
        setName(event.target.value);
    }

    return (
        <div className={"add-room-dialog"}>
            <div className={"add-room-dialog__content"}>
                <div className={"add-room-dialog__content__header"}>
                    <p className={"add-room-dialog__content__header__title"}> Create new room </p>
                </div>
                <div className={"add-room-dialog__content__body"}>
                    <p className="add-room-dialog__content__body__title"> Enter room name: </p>
                    <input type="text" className="add-room-dialog__content__body__input" value={name}
                           onChange={onNameChange}/>
                    <p className="add-room-dialog__content__body__title"> Choose default user color: </p>
                    <input className={"input-coloris"} type={"text"} value={defaultColor}
                           onChange={(color) => setDefaultColor(color)}/>
                </div>
                <div className={"add-room-dialog__content__footer"}>
                    <button className={"add-room-dialog__content__footer__button"} onClick={cancelCallback}>
                        Cancel
                    </button>
                    <button className={"add-room-dialog__content__footer__button main-button"} onClick={addRoom}>
                        Create
                    </button>
                </div>
            </div>
        </div>
    );
};

export default AddRoomDialog;