import FileDestination from "entities/fileDestination";

export interface FileInfoResult {
    bytes: number;
    url: string;
    name: string;
    contentType: string;
    uploadedTo: FileInfoDestinationResultEntry[];
}

export interface FileInfoDestinationResultEntry {
    fileDestination: FileDestination;
    description: string;
}