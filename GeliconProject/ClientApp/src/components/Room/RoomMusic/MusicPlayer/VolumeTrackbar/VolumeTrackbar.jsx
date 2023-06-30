import React from 'react';
import TrackBar from "../../../../UI/Trackbar/TrackBar";

const VolumeTrackbar = ({min, max, step, value, onChange, onSlide}) => {
    return (
        <div className={"volume-trackbar"}>
            <TrackBar
                min={min} max={max} step={step} orientation={'vertical'} direction={'rtl'}
                value={value} onChange={onChange} onSlide={onSlide}
            />
            <div className={"volume-trackbar__icon-wrapper"}>
                <img className={"volume-trackbar__icon-wrapper__icon"} src={"/source/images/volume.png"}/>
            </div>
        </div>
    );
};

export default VolumeTrackbar;