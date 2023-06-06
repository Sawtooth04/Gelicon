import {HubConnectionBuilder} from "@microsoft/signalr";

class HubConnector {
    _connection;
    connected;
    roomID;

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
        this.roomID = roomID;
        this._connection =  new HubConnectionBuilder()
            .withUrl("http://192.168.1.104:5092/hubs/room")
            .withAutomaticReconnect()
            .build();
        this.addEventHandler('Connected', () => this.connectionHandler());
        await this._connection.start();
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
}

export default HubConnector