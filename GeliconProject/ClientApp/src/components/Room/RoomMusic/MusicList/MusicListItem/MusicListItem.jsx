import React from 'react';

const MusicListItem = ({item}) => {
    return (
        <div className={"music-list__music-list-item music-list-item"}>
            <div className={"music-list-item__description"}>
                <p> {item.title} </p>
            </div>
            <div className={"music-list-item__controls"}>

            </div>
        </div>
    );
};

export default MusicListItem;