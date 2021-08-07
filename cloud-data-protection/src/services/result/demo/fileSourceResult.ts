import FileDestination from "entities/fileDestination";

export interface FileSourceResult {
    sources: FileDestinationResultEntry[];
}

export interface FileDestinationResultEntry {
    fileDestination: FileDestination;
    description: string;
}
