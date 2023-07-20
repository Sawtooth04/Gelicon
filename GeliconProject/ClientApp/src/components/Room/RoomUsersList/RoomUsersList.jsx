import React, {useState} from 'react';
import UserLabel from "./UserLabel/UserLabel";
import RoomUserEditDialog from "./RoomUserEditDialog/RoomUserEditDialog";

const RoomUsersList = ({users, roomUsersColors, onlineUsers, editableUsersID, connector, ...props}) => {
    const [roomUserEditDialogOpened, setRoomUserEditDialogOpened] = useState(false);
    const [openedEditDialogUser, setOpenedEditDialogUser] = useState(null);
    const [openedEditDialogUserColor, setOpenedEditDialogUserColor] = useState(null);

    function findUserColor(user) {
        return roomUsersColors.find(item => item.userID === user.userID).color;
    }

    function isOnline(userID) {
        return typeof(onlineUsers.find(u => u === userID)) !== 'undefined';
    }

    function isEditable(userID) {
        return typeof(editableUsersID.find(u => u === userID)) !== 'undefined';
    }

    function onUserLabelClick(isEditable, user, color) {
        if (isEditable) {
            setOpenedEditDialogUser(user);
            setOpenedEditDialogUserColor(color);
            setRoomUserEditDialogOpened(true);
        }
    }

    async function setChanges(user, color) {
        await connector.setRoomUserChanges(user, color);
        setRoomUserEditDialogOpened(false);
    }

    return (
        <div {...props}>
            {roomUserEditDialogOpened ? <RoomUserEditDialog user={openedEditDialogUser} color={openedEditDialogUserColor}
                setChangesCallback={setChanges}/> : null}
            {users.map((user) =>
                <UserLabel user={user} key={user.userID} color={findUserColor(user)} isOnline={isOnline(user.userID)}
                   isEditable={isEditable(user.userID)} onClickCallback={onUserLabelClick}/>
            )}
        </div>
    );
};

export default RoomUsersList;