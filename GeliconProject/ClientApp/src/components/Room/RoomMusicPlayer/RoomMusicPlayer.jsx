import React, {useState, useEffect} from 'react';

const RoomMusicPlayer = ({connector}) => {
    const [audio, setAudio] = useState(null);

    useEffect(() => {
        if (audio != null)
            audio.volume = 0.1;
    }, [audio]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetMusic", setMusic);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetMusic", setMusic);
        }

        //Handlers
        async function setMusic(musicID) {
            setAudio(new Audio(connector.musicRepository.getMusicStreamByID(new Map([['id', musicID]]))));
            removeEventHandlers();
        }

        async function getCurrentMusic() {
            await connector.getCurrentMusic();
        }

        if (connector.connected) {
            addEventHandlers();
            void getCurrentMusic();
        }

    }, [connector, connector.connected]);

    return (
        <div>

        </div>
    );
};

export default RoomMusicPlayer;