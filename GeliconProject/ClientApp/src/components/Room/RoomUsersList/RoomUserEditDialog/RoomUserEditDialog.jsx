import React, {useState, useEffect} from 'react';
import Coloris from "@melloware/coloris";

const RoomUserEditDialog = ({user, color, setChangesCallback}) => {
    const [changedColor, setChangedColor] = useState(color);

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
            ],
            onChange: onChangeColor
        });
    }, []);

    function setChanges() {
        setChangesCallback(user, changedColor);
    }

    function onChangeColor(color) {
        setChangedColor(color);
    }

    return (
        <div className={"edit-user-dialog"}>
            <div className={"edit-user-dialog__content"}>
                <div className={"edit-user-dialog__content__header"}>
                    <p className={"edit-user-dialog__content__header__title"}> {user.name} </p>
                </div>
                <div className={"edit-user-dialog__content__body"}>
                    <p className="edit-user-dialog__content__body__title"> Choose user color: </p>
                    <input className={"input-coloris"} type={"text"} value={changedColor} onChange={() => {}}/>
                </div>
                <div className={"edit-user-dialog__content__footer"}>
                    <button className={"edit-user-dialog__content__footer__button main-button"} onClick={setChanges}>
                        Confirm
                    </button>
                </div>
            </div>
        </div>
    );
};

export default RoomUserEditDialog;