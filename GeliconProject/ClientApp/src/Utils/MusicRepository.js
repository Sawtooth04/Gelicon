
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

        do {
            try {
                response = await queryCallback(args);
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

    switchServer() {
        if (this._currentAudioEndpointIndex === this.audioEndpoints.length - 1)
            this._currentAudioEndpointIndex = 0;
        this._currentAudioEndpointIndex++;
    }
}

export default MusicRepository