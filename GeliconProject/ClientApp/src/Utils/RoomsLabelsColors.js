class RoomsLabelsColorsHandler {
    static borderColorsNames = ["crimson", "salmon", "pink", "coral", "khaki", "plum", "purple", "wheat", "brown"];

    static getRandomBorderColor() {
        return this.borderColorsNames[Math.floor(Math.random() * this.borderColorsNames.length)];
    }
}

export default RoomsLabelsColorsHandler;