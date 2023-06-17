
class MusicRepository {
    audioEndpoints;
    mainAudioEndpoint;
    _currentAudioEndpointIndex;
    appName;

    constructor() {
        this._currentAudioEndpointIndex = 0;
    }

    async executeQuery(queryCallback, args) {
        let response;
        let bindedCallback = queryCallback.bind(this);

        do {
            try {
                response = await bindedCallback(args);
                if (!response.ok)
                    this.switchServer();
            }
            catch (e) {
                this.switchServer();
            }
        }
        while (response === null || !response.ok);
        return response;
    }

    async searchMusic(args) {
        try {
            return await fetch(
                `${this.audioEndpoints[this._currentAudioEndpointIndex]}/v1/tracks/search?query=${args.get('query')}&app_name=${this.appName}`
            );
        }
        catch (e) {
            return null;
        }
    }

    async getMusicByID(args) {
        try {
            return await fetch(
                `${this.audioEndpoints[this._currentAudioEndpointIndex]}/v1/tracks/${args.get('id')}?app_name=${this.appName}`
            );
        }
        catch (e) {
            return null;
        }
    }

    async getMusicArrayFromApi(arr) {
        let response = [];
        let executeFunction = this.executeQuery.bind(this);

        for (let i = arr.length - 1; i >= 0; i--)
            response.push(await(await executeFunction(this.getMusicByID, new Map([['id', arr[i].musicID]]))).json());
        return response;
    }

    getMusicStreamByID(args) {
        return `${this.audioEndpoints[this._currentAudioEndpointIndex]}/v1/tracks/${args.get('id')}/stream?app_name=${this.appName}`
    }

    switchServer() {
        if (this._currentAudioEndpointIndex === this.audioEndpoints.length - 1)
            this._currentAudioEndpointIndex = 0;
        this._currentAudioEndpointIndex++;
    }
}

export default MusicRepository