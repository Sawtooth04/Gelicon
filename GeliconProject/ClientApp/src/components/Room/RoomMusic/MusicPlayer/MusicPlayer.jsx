import React, {useState, useEffect} from 'react';
import VolumeTrackbar from "./VolumeTrackbar/VolumeTrackbar";
import Timeline from "./Timeline/Timeline";
import Button from "../../../UI/Button/Button";

const MusicPlayer = ({connector, setCurrentAudioInfoCallback}) => {
    const [audio, setAudio] = useState(new Audio());
    const [currentAudioInfo, setCurrentAudioInfo] = useState(null);
    const [isPlaying, setIsPlaying] = useState(false);
    const [volume, setVolume] = useState(0.1);
    const [currentTime, setCurrentTime] = useState(-1);
    const [timelineInterval, setTimelineInterval] = useState(null);
    const [isPlayNext, setIsPlayNext] = useState(true);

    useEffect(() => {
        return () => pause();
    }, []);

    useEffect(() => {
        setCurrentAudioInfoCallback(currentAudioInfo);
    }, [currentAudioInfo]);

    useEffect(() => {
        audio.onloadedmetadata = function (event) { setCurrentTime(0) } ;
        audio.volume = volume;
        if (isPlaying && audio.readyState === 4)
            play();
    }, [audio]);

    useEffect(() => {
        if (connector.connected)
            connector.addEventHandler("CurrentTimePingReceive", currentTimePingReceive);

        async function currentTimePingReceive() {
            await connector.currentTimePingResponse(currentTime);
        }

        return () => connector.removeEventHandler("CurrentTimePingReceive", currentTimePingReceive);
    }, [currentTime]);

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
            connector.addEventHandler("SetAudioTime", setAudioTime);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetMusic", setMusic);
            connector.removeEventHandler("SetAudioTime", setAudioTime);
        }

        //Handlers
        async function setMusic(music) {
            await setMusicSources(music);
        }

        async function getCurrentMusic() {
            await connector.getCurrentMusic();
        }

        function setAudioTime(value) {
            setCurrentTime(value);
            audio.currentTime = value;
        }

        if (connector.connected) {
            addEventHandlers();
            void getCurrentMusic();
        }
        return () => removeEventHandlers();
    }, [connector, connector.connected]);

    useEffect(() => {
        function addEventHandlers() {
            connector.addEventHandler("SetPlayState", setPlayState);
            connector.addEventHandler("SetPauseState", setPauseState);
            connector.addEventHandler("SetMusicAfterInit", setMusic);
            connector.addEventHandler("SetAudioTime", setAudioTime);
            connector.addEventHandler("SetSpeedRatio", setSpeedRatio);
            connector.addEventHandler("SetPlayNextState", setPlayNextState);
            connector.addEventHandler("SetPlayLoopState", setPlayLoopState);
            audio.addEventListener('ended', setAutoplayNextMusic);
        }

        function removeEventHandlers() {
            connector.removeEventHandler("SetPlayState", setPlayState);
            connector.removeEventHandler("SetPauseState", setPauseState);
            connector.removeEventHandler("SetMusicAfterInit", setMusic);
            connector.removeEventHandler("SetAudioTime", setAudioTime);
            connector.removeEventHandler("SetSpeedRatio", setSpeedRatio);
            connector.removeEventHandler("SetPlayNextState", setPlayNextState);
            connector.removeEventHandler("SetPlayLoopState", setPlayLoopState);
            audio.removeEventListener('ended', setAutoplayNextMusic);
        }

        //Handlers
        function setPlayState() {
            setIsPlaying(true);
        }

        function setPauseState() {
            setIsPlaying(false);
        }

        async function setMusic(music) {
            if (!audio.paused)
                pause();
            await setMusicSources(music);
            audio.load();
            if (isPlaying && audio.paused)
                await play();
        }

        async function setAudioTime(value) {
            setCurrentTime(value);
            audio.currentTime = value;
            if (isPlaying)
                await play();
        }

        function setSpeedRatio(value) {
            audio.playbackRate = value;
        }

        function setPlayNextState() {
            setIsPlayNext(true);
        }

        function setPlayLoopState() {
            setIsPlayNext(false);
        }

        if (connector.connected)
            addEventHandlers();
        return () => removeEventHandlers();
    }, [connector, connector.connected, audio, isPlaying, isPlayNext, volume, timelineInterval]);

    async function setMusicSources(music) {
        if (music != null) {
            let response = await connector.musicRepository.executeQuery(
                connector.musicRepository.getMusicByID,
                new Map([['id', music.musicID]])
            );
            audio.src = connector.musicRepository.getMusicStreamByID(new Map([['id', music.musicID]]));
            setCurrentAudioInfo((await response.json()).data);
        }
    }

    async function play() {
        await audio.play();
        setTimelineInterval(setInterval(() => {
            setCurrentTime(audio.currentTime);
        }, 1000));
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

    async function setAutoplayNextMusic() {
        await connector.setAutoplayNextMusic(currentAudioInfo.id);
    }

    async function playLoopCallback() {
        await connector.setPlayLoopState();
    }

    async function playNextCallback() {
        await connector.setPlayNextState();
    }

    return (
        <div className={"room-music__music-player music-player"}>
            {(currentAudioInfo != null) ?
                <div className={"music-player__info"}>
                    <div className={"music-player__info__logo"}>
                        <img src={currentAudioInfo.artwork['150x150']}/>
                    </div>
                    <p> {currentAudioInfo.title} </p>
                </div> : null
            }
            <div className={"music-player__controls"}>
                <Button onClick={previousCallback} src={"/source/images/previous.png"}/>
                {(!isPlaying) ?
                    <Button className={"button_bordered"} onClick={playCallback} src={"/source/images/play.png"}/> :
                    <Button className={"button_bordered"} onClick={pauseCallback} src={"/source/images/pause.png"}/>
                }
                <Button onClick={nextCallback} src={"/source/images/next.png"}/>
                <Timeline
                    min={0} max={(!isNaN(audio.duration)) ? audio.duration : 0} step={1} value={currentTime}
                    onChange={setCurrentTimeCallback} onSlide={setAudioTimeCallback}
                />
                <VolumeTrackbar
                    min={0} max={1} step={0.01} value={volume} onChange={setVolume} onSlide={setAudioVolumeCallback}
                />
                {(isPlayNext) ?
                    <Button onClick={playLoopCallback} src={"/source/images/sequentially.png"}/> :
                    <Button onClick={playNextCallback} src={"/source/images/loop.png"}/>
                }
            </div>
        </div>
    );
};

export default MusicPlayer;