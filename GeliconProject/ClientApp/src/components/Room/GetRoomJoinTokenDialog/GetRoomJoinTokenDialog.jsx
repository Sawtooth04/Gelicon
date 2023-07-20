import React, {useState, useEffect} from 'react';

const GetRoomJoinTokenDialog = ({roomID, closeRoomJoinTokenDialog}) => {
    const [token, setToken] = useState('');
    const [isCopied, setIsCopied] = useState(false);

    useEffect(() => {
        async function init() {
            let response = await fetch('/rooms/get-join-room-link?' + new URLSearchParams({roomID: roomID}), {method: "get"});
            setToken(await response.json());
        }

        void init();
    }, []);

    async function onCopy() {
        if (window.isSecureContext) {
            await navigator.clipboard.writeText(token);
            setIsCopied(true);
        }
    }

    return (
        <div className={"get-room-join-token-dialog"}>
            <div className={"get-room-join-token-dialog__content"}>
                <div className={"get-room-join-token-dialog__content__header"}>
                    <p className={"get-room-join-token-dialog__content__header__title"}> Room join token </p>
                </div>
                <div className={"get-room-join-token-dialog__content__body"}>
                    <p className={"get-room-join-token-dialog__content__body__title"}> { token } </p>
                    <p className={"get-room-join-token-dialog__content__body__title_small"}>
                        Note: the token is valid for 30 minutes.
                    </p>
                    {isCopied ?
                        <p className={"get-room-join-token-dialog__content__body__title_small title-animated"}>
                            Successfully copied to clipboard!
                        </p> : null
                    }
                </div>
                <div className={"get-room-join-token-dialog__content__footer"}>
                    <button className={"get-room-join-token-dialog__content__footer__button"} onClick={closeRoomJoinTokenDialog}>
                        Close
                    </button>
                    <button className={"get-room-join-token-dialog__content__footer__button main-button"} onClick={onCopy}>
                        Copy
                    </button>
                </div>
            </div>
        </div>
    );
};

export default GetRoomJoinTokenDialog;