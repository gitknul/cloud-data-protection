export abstract class ServiceBase {
    protected onError(error: any) {
        if (error.__CANCEL__) {
            return Promise.resolve();
        }

        const response = error.response;

        if (response) {
            let message = response.data.message || response.data.detail || response.data.title;
            let status = response.data.statusDescription || response.statusText;

            if (response.status === 400) {
                for (const [_, value] of Object.entries(response.data.errors)) {
                    message += '\n' + value;
                }
            }

            return Promise.reject(status + ': ' + message);
        }

        // The API could not the contacted
        // This means either the API is down, or the user has no connection
        return ServiceBase.unknownError();
    }

    private static unknownError(): Promise<string> {
        return Promise.reject('An unknown error has occurred. Please try again later.')
    }
}