declare global {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    interface Array<T> {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        joinNice<T>(separator: string, lastSeparator: string): string;
    }
}

// eslint-disable-next-line no-extend-native
Array.prototype.joinNice = function(separator: string, lastSeparator: string): string {
    if (this.length === 0) {
        return '';
    }

    if (this.length === 1) {
        return this[0] as string;
    }

    if (this.length === 2) {
        return this.join(lastSeparator);
    }

    const firstPart = this.slice(0, this.length - 1).join(separator);

    return firstPart + lastSeparator + this[this.length - 1];
}

export {}