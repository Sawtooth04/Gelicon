import {HubConnectionBuilder} from "@microsoft/signalr";

class HubConnector {
    _connection;
    connected;

    constructor() {
        this._connection = null;
        this.connected = false;
    }

    addEventHandler(event, handler) {
        this._connection.on(event, handler);
    }

    removeEventHandler(event, handler) {
        this._connection.off(event, handler);
    }

    async setConnectionToHub(roomID) {
        this._connection =  new HubConnectionBuilder()
            .withUrl("http://192.168.1.104:5092/hubs/room")
            .withAutomaticReconnect()
            .build();
        await this._connection.start();
        this._connection.onclose(this.onDisconnected);
        this.connected = true;
        this.roomInit(roomID);
    }

    roomInit(roomID) {
        void this._connection.invoke("Init", roomID);
    }

    async sendMessage(message, roomID) {
        await this._connection.invoke("Send", message, roomID);
    }
}

export default HubConnector