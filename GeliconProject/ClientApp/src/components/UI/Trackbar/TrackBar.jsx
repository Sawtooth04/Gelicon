import React, {useEffect, useRef} from 'react';
import noUiSlider from 'nouislider';
import 'nouislider/dist/nouislider.css';

const TrackBar = ({min, max, step, orientation, direction, value, onChange, onSlide}) => {
    const trackbar = useRef(null);

    useEffect(() => {
        if (trackbar.current != null && trackbar.current.noUiSlider != null)
            trackbar.current.noUiSlider.destroy();
        noUiSlider.create(trackbar.current, {
            start: [value],
            orientation: orientation,
            direction: direction,
            connect: [true, false],
            range: { min: min, max: max },
            step: step,
        });
        trackbar.current.noUiSlider.on('change', onChangeCallback);
        trackbar.current.noUiSlider.on('slide', onSlideCallback);
    }, [value]);

    function onChangeCallback() {
        if (Number(trackbar.current.noUiSlider.get()) !== value) {
            trackbar.current.noUiSlider.off('change');
            trackbar.current.noUiSlider.off('drag');
            onChange(Number(trackbar.current.noUiSlider.get()));
        }
    }

    function onSlideCallback() {
        if (typeof(onSlide) != 'undefined' && Number(trackbar.current.noUiSlider.get()) !== value)
            onSlide(Number(trackbar.current.noUiSlider.get()));
    }

    return (
        <div className={"trackbar"}>
            <div className={"trackbar__wrapper"} ref={trackbar}>
            </div>
        </div>
    );
};

export default TrackBar;