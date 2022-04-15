export const formatEnum = (value: any, type: any): string => {
    const enumString = type[value].toString();

    const parts = [];

    let firstIndex = -1;

    for (let i = 0; i < enumString.length; i++) {
        const c = enumString.charAt(i);
        const isUpperCase = c !== c.toLocaleLowerCase();

        if (isUpperCase && firstIndex === -1) {
            firstIndex = i;
        } else if (isUpperCase) {
            const part = enumString.substring(firstIndex, i);

            if (firstIndex !== 0) {
                parts.push(part.toLowerCase());
            } else {
                parts.push(part);
            }

            firstIndex = i;
        }

        if (i === enumString.length -1) {
            const part = enumString.substring(firstIndex);

            if (parts.length === 0) {
                parts.push(part);
            } else {
                parts.push(part.toLowerCase());
            }
        }
    }

    return parts.join(' ');
}