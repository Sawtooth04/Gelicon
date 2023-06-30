import React, {useState, useEffect} from 'react';
import VolumeTrackbar from "./VolumeTrackbar/VolumeTrackbar";
import Timeline from "./Timeline/Timeline";

const MusicPlayer = ({connector, setCurrentAudioInfoCallback}) => {
    const [audio, setAudio] = useState(new Audio());
    const [currentAudioInfo, setCurrentAudioInfo] = useState(null);
    const [isPlaying, setIsPlaying] = useState(false);
    const [volume, setVolume] = useState(0.1);
    const [currentTime, setCurrentTime] = useState(0);
    const [timelineInterval, setTimelineInterval] = useState(null);

    useEffect(() => {
        setCurrentAudioInfoCallback(currentAudioInfo);
    }, [currentAudioInfo]);

    useEffect(() => {
        audio.volume = volume;
        if (isPlaying && audio.readyState === 4)
            audio.play();
    }, [audio]);

    useEffect(() => {
        try {
            if (isPlaying && audio.paused)
                play();
            else if (!audio.paused)
                pause();
        }
        catch (e) {
            connector.musicRepository.switchServer();
        }
    }, [isPlaying]);

    useEffect(() => {
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
            await setMusicSources(music);
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
            connector.addEventHandler("SetMusicAfterInit", setMusic);
            connector.addEventHandler("SetAudioTime", setAudioTime);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetPlayState", setPlayState);
            connector.removeEventHandler("SetPauseState", setPauseState);
            connector.removeEventHandler("SetMusic", setMusic);
            connector.removeEventHandler("SetAudioTime", setAudioTime);
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
            if (!audio.paused)
                audio.pause();
            await setMusicSources(music);
            audio.load();
            if (isPlaying && audio.paused)
                await audio.play();
        }

        function setAudioTime(value) {
            setCurrentTime(value);
            audio.currentTime = value;
        }

        if (connector.connected)
            addEventHandlers();
    }, [connector, connector.connected, audio, isPlaying, volume]);

    async function setMusicSources(music) {
        let response = await connector.musicRepository.executeQuery(
            connector.musicRepository.getMusicByID,
            new Map([['id', music.musicID]])
        );
        setCurrentAudioInfo((await response.json()).data);
        audio.src = connector.musicRepository.getMusicStreamByID(new Map([['id', music.musicID]]));
    }

    function play() {
        audio.play();
        setTimelineInterval(setInterval(() => setCurrentTime(audio.currentTime), 1000));
    }

    function pause() {
        audio.pause();
        clearInterval(timelineInterval);
    }

    async function playCallback() {
        if (!isPlaying)
            await connector.setPlayState();
    }

    async function pauseCallback() {
        if (isPlaying)
            await connector.setPauseState();
    }

    async function previousCallback() {
        await connector.setPreviousMusic();
    }

    async function nextCallback() {
        await connector.setNextMusic();
    }

    function setAudioVolumeCallback(value) {
        audio.volume = value;
    }

    async function setCurrentTimeCallback(value) {
        setTimelineInterval(setInterval(() => setCurrentTime(audio.currentTime), 1000));
        await connector.setAudioTime(value);
    }

    function setAudioTimeCallback(value) {
        clearInterval(timelineInterval);
        audio.currentTime = value;
    }

    return (
        <div className={"room-music__music-player music-player"}>
            <div className={"music-player__info"}>
                {
                    (currentAudioInfo != null) ?
                        <p> {currentAudioInfo.title} </p> : <p></p>
                }
            </div>
            <div className={"music-player__controls"}>
                <button className={"music-player__controls__button"} onClick={previousCallback}>
                    <img src={"/source/images/previous.png"}/>
                </button>
                {(!isPlaying) ?
                    <button className={"music-player__controls__button button_bordered"} onClick={playCallback}>
                        <img src={"/source/images/play.png"}/>
                    </button> :
                    <button className={"music-player__controls__button button_bordered"} onClick={pauseCallback}>
                        <img src={"/source/images/pause.png"}/>
                    </button>
                }
                <button className={"music-player__controls__button"} onClick={nextCallback}>
                    <img src={"/source/images/next.png"}/>
                </button>
                <Timeline
                    min={0} max={(!isNaN(audio.duration)) ? audio.duration : 0} step={1} value={currentTime}
                    onChange={setCurrentTimeCallback} onSlide={setAudioTimeCallback}
                />
                <VolumeTrackbar
                    min={0} max={1} step={0.01} value={volume} onChange={setVolume} onSlide={setAudioVolumeCallback}
                />
            </div>
        </div>
    );
};

export default MusicPlayer;