import React from 'react';
import UserLabel from "./UserLabel/UserLabel";

const RoomUsersList = ({users, roomUsersColors, ...props}) => {
    function findUserColor(user) {
        return roomUsersColors.find(item => item.userID === user.userID).color;
    }

    return (
        <div {...props}>
            {users.map((user) =>
                <UserLabel user={user} key={user.userID} color={findUserColor(user)}/>
            )}
        </div>
    );
};

export default RoomUsersList;