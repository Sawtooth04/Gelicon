import {HubConnectionBuilder} from "@microsoft/signalr";
import MusicRepository from "./MusicRepository";

class HubConnector {
    _connection;
    connected;
    roomID;
    musicRepository;

    constructor() {
        this._connection = null;
        this.connected = false;
        this.musicRepository = new MusicRepository();
    }

    addEventHandler(event, handler) {
        this._connection.on(event, handler);
    }

    removeEventHandler(event, handler) {
        this._connection.off(event, handler);
    }

    async setConnectionToHub(roomID) {
        this.roomID = roomID;
        this._connection =  new HubConnectionBuilder()
            .withUrl("http://192.168.1.104:5092/hubs/room")
            .withAutomaticReconnect()
            .build();
        this.addEventHandler('Connected', () => this.connectionHandler());
        this.addEventHandler('SetMainAudioEndpoint', (endpoint) => this.setMainAudioEndpoint(endpoint));
        this.addEventHandler('SetAppName', (appName) => this.setAppName(appName));
        await this._connection.start();
        this.musicRepository.audioEndpoints = (await this.getAudioEndpoints()).data;
        await this.getAppName();
        this.connected = true;
    }

    async sendMessage(message, roomID) {
        await this._connection.invoke("SendMessage", message, roomID);
    }

    async deleteMessage(key, roomID) {
        await this._connection.invoke("DeleteMessage", key, roomID);
    }

    async pingResponse() {
        await this._connection.invoke("PingResponse", this.roomID);
    }

    async connectionHandler() {
        await this._connection.invoke("UserConnect", this.roomID);
    }

    disconnect() {
        this._connection.stop();
        this.connected = false;
    }

    setMainAudioEndpoint(endpoint) {
        this.musicRepository.mainAudioEndpoint = endpoint;
    }

    setAppName(appName) {
        this.musicRepository.appName = appName;
    }

    async getAudioEndpoints() {
        await this._connection.invoke("GetMainAudioEndpoint");
        return await (await fetch(this.musicRepository.mainAudioEndpoint)).json();
    }

    async getAppName() {
        await this._connection.invoke("GetAppName");
    }

    async addMusicToRoom(musicID) {
        await this._connection.invoke("AddMusicToRoom", this.roomID, musicID);
    }

    async addPlaylistToRoom(playlist) {
        await this._connection.invoke("AddPlaylist", this.roomID, playlist.name);
    }

    async getRoomMusicList() {
        await this._connection.invoke("GetRoomMusicList", this.roomID);
    }

    async getPlaylists() {
        await this._connection.invoke("GetPlaylists", this.roomID);
    }

    async getOnlineUsersList() {
        await this._connection.invoke("GetOnlineUsersList", this.roomID);
    }

    async deleteRoomMusic(musicID) {
        await this._connection.invoke("DeleteRoomMusic", this.roomID, musicID);
    }

    async deletePlaylist(playlist) {
        await this._connection.invoke("DeletePlaylist", this.roomID, playlist.roomPlaylistID);
    }

    async currentTimePingResponse(value) {
        await this._connection.invoke("CurrentTimePingResponse", this.roomID, parseInt(value, 10));
    }

    async setRoomUserChanges(user, color) {
        await this._connection.invoke("SetRoomUserChanges", this.roomID, user.userID, color);
    }

    //Music player
    async getCurrentMusic() {
        await this._connection.invoke("GetCurrentMusic", this.roomID);
    }

    async setRoomMusic(musicID) {
        await this._connection.invoke("SetRoomMusic", this.roomID, musicID);
    }

    async setPlayState() {
        await this._connection.invoke("SetPlayState", this.roomID);
    }

    async setPauseState() {
        await this._connection.invoke("SetPauseState", this.roomID);
    }

    async setNextMusic() {
        await this._connection.invoke("SetNextMusic", this.roomID);
    }

    async setPreviousMusic() {
        await this._connection.invoke("SetPreviousMusic", this.roomID);
    }

    async setAudioTime(value) {
        await this._connection.invoke("SetAudioTime", this.roomID, value);
    }

    async setAutoplayNextMusic(musicID) {
        await this._connection.invoke("SetAutoplayNextMusic", this.roomID, musicID);
    }

    async setPlayNextState() {
        await this._connection.invoke("SetPlayNextState", this.roomID);
    }

    async setPlayLoopState() {
        await this._connection.invoke("SetPlayLoopState", this.roomID);
    }
}

export default HubConnector