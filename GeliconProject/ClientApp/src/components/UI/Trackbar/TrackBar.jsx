import React, {useEffect, useRef} from 'react';
import noUiSlider from 'nouislider';
import 'nouislider/dist/nouislider.css';

const TrackBar = ({min, max, step, orientation, direction, value, onChangeCallback}) => {
    const trackbar = useRef(null);

    useEffect(() => {
        noUiSlider.create(trackbar.current, {
            start: [value],
            orientation: orientation,
            direction: direction,
            connect: [true, false],
            range: { min: min, max: max },
            step: step,
        });
        trackbar.current.noUiSlider.on('change', onChange);
    }, [value]);

    function onChange() {
        if (Number(trackbar.current.noUiSlider.get()) !== value) {
            trackbar.current.noUiSlider.off('change');
            onChangeCallback(Number(trackbar.current.noUiSlider.get()));
            trackbar.current.noUiSlider.destroy();
        }
    }

    return (
        <div className={"trackbar"}>
            <div className={"trackbar__wrapper"} ref={trackbar}>
            </div>
        </div>
    );
};

export default TrackBar;