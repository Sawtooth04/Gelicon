import React, {useState} from 'react';
import TextArea from "../../../UI/TextArea/TextArea";
import Button from "../../../UI/Button/Button";

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
                <Button onClick={sendMessage} src={"/source/images/send.png"}/>
            </div>
        </div>
    );
};

export default RoomChatControls;