import React, {useEffect} from 'react';

const TextArea = ({inputCallback, value, className, ...props}) => {
    const textArea = React.useRef(null);

    useEffect(() => {
        resizeTextArea();
    }, [value]);

    function resizeTextArea(maxHeight = document.documentElement.offsetHeight * 0.3) {
        textArea.current.style.height = 'auto';
        if (textArea.current.scrollHeight <= maxHeight)
            textArea.current.style.height = textArea.current.scrollHeight + 'px';
        else
            textArea.current.style.height = maxHeight + 'px';
    }

    function onInput() {
        inputCallback(textArea.current.value);
        resizeTextArea();
    }

    return (
        <textarea className={`${className} ui-textarea`} {...props} value={value} ref={textArea} onInput={onInput}/>
    );
};

export default TextArea;