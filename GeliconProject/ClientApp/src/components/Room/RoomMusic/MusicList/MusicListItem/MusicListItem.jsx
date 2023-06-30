import React from 'react';

const MusicListItem = ({item, current, onItemClick, onDelete}) => {
    function onClick() {
        onItemClick(item);
    }

    function onDeleteClick() {
        onDelete(item);
    }

    return (
        <div className={`music-list__music-list-item music-list-item ${(current) ? 'music-list-item_current' : ''}`} onClick={onClick}>
            <div className={"music-list-item__description"}>
                <p> {item.title} </p>
            </div>
            <div className={"music-list-item__controls"}>
                <button className={"music-list-item__controls__button"} onClick={onDeleteClick}>
                    <img src={"/source/images/previous.png"}/>
                </button>
            </div>
        </div>
    );
};

export default MusicListItem;