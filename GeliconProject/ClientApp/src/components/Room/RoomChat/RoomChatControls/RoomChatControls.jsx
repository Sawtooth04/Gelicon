import React, {useState} from 'react';
import TextArea from "../../../UI/TextArea/TextArea";

const RoomChatControls = (props) => {
    const [message, setMessage] = useState("");

    async function sendMessage(event) {
        event.preventDefault();
        await props.sendMessage(message);
        setMessage("");
    }

    function onInput(message) {
        setMessage(message);
    }

    return (
        <div className="chat__controls controls">
            <TextArea className="controls__message-input" value={message} inputCallback={onInput}/>
            <div className="controls__buttons-wrapper">
                <button className="controls__submit-button" onClick={sendMessage}> </button>
            </div>
        </div>
    );
};

export default RoomChatControls;