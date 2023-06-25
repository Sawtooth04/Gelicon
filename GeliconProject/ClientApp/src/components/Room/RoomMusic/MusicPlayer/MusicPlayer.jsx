import React, {useState, useEffect} from 'react';
import VolumeTrackbar from "./VolumeTrackbar/VolumeTrackbar";

const MusicPlayer = ({connector}) => {
    const [audio, setAudio] = useState(null);
    const [isPlaying, setIsPlaying] = useState(false);
    const [volume, setVolume] = useState(0.1);

    useEffect(() => {
        if (audio != null) {
            audio.volume = volume;
            if (isPlaying)
                audio.play();
        }
    }, [audio]);

    useEffect(() => {
        try {
            if (audio != null) {
                if (isPlaying)
                    audio.play();
                else
                    audio.pause();
            }
        }
        catch (e) {
            connector.musicRepository.switchServer();
        }
    }, [isPlaying]);

    useEffect(() => {
        if (audio != null)
            audio.volume = volume;
    }, [volume]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetMusic", setMusic);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetMusic", setMusic);
        }

        //Handlers
        async function setMusic(music) {
            setAudio(new Audio(connector.musicRepository.getMusicStreamByID(new Map([['id', music.musicID]]))));
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

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetPlayState", setPlayState);
            connector.addEventHandler("SetPauseState", setPauseState);
            connector.addEventHandler("SetMusic", setMusic);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetPlayState", setPlayState);
            connector.removeEventHandler("SetPauseState", setPauseState);
            connector.removeEventHandler("SetMusic", setMusic);
        }

        //Handlers
        function setPlayState() {
            setIsPlaying(true);
            removeEventHandlers();
        }

        function setPauseState() {
            setIsPlaying(false);
            removeEventHandlers();
        }

        async function setMusic(music) {
            if (audio != null)
                audio.pause();
            setAudio(new Audio(connector.musicRepository.getMusicStreamByID(new Map([['id', music.musicID]]))));
            removeEventHandlers();
        }

        if (connector.connected)
            addEventHandlers();
    }, [connector, connector.connected, audio, isPlaying, volume]);

    async function play() {
        if (!isPlaying)
            await connector.setPlayState();
    }

    async function pause() {
        if (isPlaying)
            await connector.setPauseState();
    }

    async function previous() {
        await connector.setPreviousMusic();
    }

    async function next() {
        await connector.setNextMusic();
    }

    return (
        <div className={"room-music__music-player music-player"}>
            <div className={"music-player__controls"}>
                <button onClick={previous}> Previous </button>
                {(!isPlaying) ?
                    <button onClick={play}> Play </button> :
                    <button onClick={pause}> Pause </button>
                }
                <button onClick={next}> Next </button>
                <VolumeTrackbar min={0} max={1} step={0.01} value={volume} onChangeCallback={setVolume}/>
            </div>
        </div>
    );
};

export default MusicPlayer;