import React, {useEffect} from 'react';
import TrackBar from "../../../../UI/Trackbar/TrackBar";

const Timeline = ({min, max, step, value, onChange, onSlide}) => {
    return (
        <div className={"timeline"}>
            <TrackBar min={min} max={max} step={step} orientation={'horizontal'} direction={'ltr'}
                      value={value} onChange={onChange} onSlide={onSlide}/>
        </div>
    );
};

export default Timeline;