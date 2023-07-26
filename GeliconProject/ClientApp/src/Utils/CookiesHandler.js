class CookiesHandler {
    static getCookie(name) {
        let regex = new RegExp(/[;=\s]/), split = document.cookie.split(regex), i = split.indexOf(name);
        return (i === -1) ? null : split[i + 1];
    }
}

export default CookiesHandler;