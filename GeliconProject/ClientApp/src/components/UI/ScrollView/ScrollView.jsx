import React, {useState, useEffect, useRef} from 'react';

const ScrollView = ({elements, threshold, nextCallback, prevCallback, loadingState}) => {
    const view = useRef(null);
    const [lastCompareResult, setLastCompareResult] = useState(-1);

    useEffect(() => {
        function compareScroll() {
            if (view.current.scrollTop + view.current.clientHeight > view.current.scrollHeight * threshold)
                return 1;
            else if (view.current.scrollTop < view.current.scrollHeight * (1 - threshold))
                return -1;
            else return 0;
        }

        function onScroll() {
            if (view.current !== null && !loadingState) {
                let compareResult = compareScroll();
                if (lastCompareResult !== compareResult) {
                    if (compareResult === -1)
                        prevCallback();
                    else if (compareResult === 1)
                        nextCallback();
                    setLastCompareResult(compareResult);
                }
            }
        }

        if (view.current != null) {
            view.current.addEventListener("scroll", onScroll);
            return () => {
                if (view.current != null)
                    view.current.removeEventListener("scroll", onScroll);
            }
        }
    }, [elements, lastCompareResult]);

    useEffect(() => {
        if (!loadingState && view.current.scrollHeight === view.current.clientHeight)
            nextCallback();
    }, [elements]);

    return (
        <div className="scroll-view" ref={view}>
            {elements}
        </div>
    );
};

export default ScrollView;