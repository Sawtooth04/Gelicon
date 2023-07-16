import React from 'react';
import UserLabel from "./UserLabel/UserLabel";

const RoomUsersList = ({users, roomUsersColors, onlineUsers, ...props}) => {
    function findUserColor(user) {
        return roomUsersColors.find(item => item.userID === user.userID).color;
    }

    function isOnline(userID) {
        return typeof(onlineUsers.find(u => u === userID)) !== 'undefined';
    }

    return (
        <div {...props}>
            {users.map((user) =>
                <UserLabel user={user} key={user.userID} color={findUserColor(user)} isOnline={isOnline(user.userID)}/>
            )}
        </div>
    );
};

export default RoomUsersList;