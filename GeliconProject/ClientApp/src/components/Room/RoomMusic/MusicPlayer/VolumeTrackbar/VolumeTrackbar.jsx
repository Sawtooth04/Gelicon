import React from 'react';
import TrackBar from "../../../../UI/Trackbar/TrackBar";

const VolumeTrackbar = ({min, max, step, value, onChangeCallback}) => {
    return (
        <div className={"volume-trackbar"}>
            <TrackBar min={min} max={max} step={step} orientation={'vertical'} direction={'rtl'}
                value={value} onChangeCallback={onChangeCallback}/>
            <div className={"volume-trackbar__icon-wrapper"}>
                <img className={"volume-trackbar__icon-wrapper__icon"} src={"/source/images/add-music.png"}/>
            </div>
        </div>
    );
};

export default VolumeTrackbar;