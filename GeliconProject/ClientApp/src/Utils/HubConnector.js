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
        await this._connection.invoke("Send", message, roomID);
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

    async getRoomMusic() {
        await this._connection.invoke("GetRoomMusic", this.roomID);
    }

    async getCurrentMusic() {
        await this._connection.invoke("GetCurrentMusic", this.roomID);
    }
}

export default HubConnector