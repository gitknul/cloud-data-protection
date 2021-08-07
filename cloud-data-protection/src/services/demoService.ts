import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import {http} from "common/http";
import {FileDownloadResult} from "services/result/demo/fileDownloadResult";
import {FileUploadResult} from "services/result/demo/fileUploadResult";
import {FileInfoResult} from "services/result/demo/fileInfoResult";
import {readFileName} from "common/parser/contentDisposition";
import fileDownload from "js-file-download";
import {FileSourceResult} from "services/result/demo/fileSourceResult";
import FileUploadInput from "./input/demo/fileUploadInput";

class DemoService extends ServiceBase {
    /* 25 mb */
    public static readonly maxFileSize: number = 25 * 1024 * 1024;

    public async upload(file: File, input: FileUploadInput, cancelToken?: CancelToken): Promise<FileUploadResult> {
        const formData = new FormData();

        formData.append('File', file);
        formData.append('Input', JSON.stringify(input));

        return await http.post('/demo/file', formData, { cancelToken: cancelToken, headers: { 'content-type': 'multipart/form-data' } })
            .then((response: AxiosResponse<FileUploadResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async getFileInfo(id: string, cancelToken?: CancelToken): Promise<FileInfoResult> {
        return await http.get('/demo/file', { cancelToken: cancelToken, params: { id: id } })
            .then((response: AxiosResponse<FileInfoResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async getDestinations(cancelToken?: CancelToken): Promise<FileSourceResult> {
            return await http.get('/demo/filedestination', { cancelToken: cancelToken })
            .then((response: AxiosResponse<FileSourceResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async downloadFile(id: string, cancelToken?: CancelToken): Promise<FileDownloadResult> {
        const headers = { 'accept' : 'application/octet-stream'}
        const responseType = 'blob';

        return await http.get('/demo/file/download', { cancelToken: cancelToken, params: { id: id }, headers: headers, responseType: responseType })
            .then((response: AxiosResponse) => {
                fileDownload(response.data, readFileName(response.headers['content-disposition']), response.data.contentType);

                const result = {
                    name: response.headers['x-file-name'],
                    downloadedFrom: response.headers['x-file-downloaded-from'],
                }

                return Promise.resolve(result);
            })
            .catch((e: any) => this.onError(e));
    }
}

export default DemoService;