import {
    Button,
    Checkbox, FormControlLabel,
    FormGroup, FormLabel,
    Input,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    Typography
} from "@material-ui/core";
import React, {FormEvent, useEffect, useState} from "react";
import {formatBytes} from "common/formatting/fileFormat";
import {CancelTokenSource} from "axios";
import {http} from "common/http";
import DemoService from "services/demoService";
import {useSnackbar} from "notistack";
import {startLoading, stopLoading} from "common/progress/helper";
import snackbarOptions from "common/snackbar/options";
import {FileDownloadResult} from "services/result/demo/fileDownloadResult";
import {FileUploadResult} from "services/result/demo/fileUploadResult";
import {FileInfoResult} from "services/result/demo/fileInfoResult";
import {CloudOutlined, Crop, Description, Info} from "@material-ui/icons";
import {FileSourceResult, FileDestinationResultEntry} from "services/result/demo/fileSourceResult";
import FileDestination from "entities/fileDestination";
import FileUploadInput from "services/input/demo/fileUploadInput";
import "extensions/array";
import './demo.css';

const Demo = () => {
    const [selectedFile, setSelectedFile] = useState<File>();
    const [uploadedFile, setUploadedFile] = useState<FileUploadResult>();

    const [fileUploadInput, setFileUploadInput] = useState<FileUploadInput>({ destinations: [] });

    const [sources, setDestinations] = useState<FileSourceResult>();

    const [fileId, setFileId] = useState('');
    const [fileInfo, setFileInfo] = useState<FileInfoResult>();

    const [initialized, setInitialized] = useState<boolean>(false);

    const { enqueueSnackbar } = useSnackbar();

    const demoService = new DemoService();

    let cancelTokenSource: CancelTokenSource;

    useEffect(() => {
        return () => {
            cancelTokenSource?.cancel();
        }
    })

    useEffect(() => {
        if (!initialized) {
            startLoading();

            getSources()
                .catch(e => onError(e))
                .finally(() => setInitialized(true))
                .finally(() => stopLoading());

            return;
        }

        startLoading();

        onFileIdChange();
    }, [fileId])

    const getSources = async () => {
       return await demoService.getDestinations()
            .then(result => setDestinations(result));
    }

    const onDestinationChange = (e: any) => {
        let destinations = fileUploadInput.destinations.slice();

        const fileSource: FileDestination = parseInt(e.target.name);

        if (destinations.indexOf(fileSource) === -1) {
            destinations.push(fileSource);
        } else {
            destinations = destinations.filter(d => d !== fileSource);
        }

        setFileUploadInput({destinations: destinations});
    }

    const isDestinationChecked = (entry: FileDestinationResultEntry) => {
        return fileUploadInput.destinations.indexOf(entry.fileDestination) !== -1;
    }

    const canSubmit = () => {
        if (selectedFile === undefined) {
            return false;
        }

        if (fileUploadInput.destinations.length === 0) {
            return false;
        }

        return true;
    }

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault();

        if (!selectedFile) {
            enqueueSnackbar('Please upload a file', snackbarOptions);
            return;
        }

        if (selectedFile.size > DemoService.maxFileSize) {
            enqueueSnackbar('The selected file is too big. The upload limit is 25MB.', snackbarOptions);
            return;
        }

        cancelTokenSource = http.CancelToken.source();

        startLoading();

        await demoService.upload(selectedFile, fileUploadInput, cancelTokenSource.token)
            .then((result) => setUploadedFile(result))
            .then(() => setSelectedFile(undefined))
            .catch((e) => onError(e))
            .finally(() => stopLoading());
    }

    const uploadedTo = () => {
        if (!uploadedFile) {
            return '';
        }

        return uploadedFile.uploadedTo
            .filter(u => u.success)
            .map(u => u.description)
            .sort()
            .joinNice(', ', ' and ');
    }

    const uploadedToError = () => {
        if (!uploadedFile) {
            return '';
        }

        return uploadedFile.uploadedTo
            .filter(u => !u.success)
            .map(u => u.description)
            .joinNice(', ', ' and ');
    }

    const onFileSelect = (e: any) => {
        const file: File = e.target.files[0];

        if (file.size > DemoService.maxFileSize) {
            enqueueSnackbar('The selected file is too big. The upload limit is 25MB.', snackbarOptions);

            setSelectedFile(undefined);

            e.target.value = null;

            return;
        }

        setSelectedFile(e.target.files[0]);
    }

    const onFileIdChange = async () => {
        cancelTokenSource = http.CancelToken.source();

        startLoading();

        await demoService.getFileInfo(fileId, cancelTokenSource.token)
            .then((result) => setFileInfo(result))
            .catch(() => setFileInfo(undefined))
            .finally(() => stopLoading());
    }

    const copyToClipboard = async (e: any) => {
        await navigator.clipboard.writeText(e.target.innerText);

        enqueueSnackbar('Code has been copied to the clipboard', { ...snackbarOptions, autoHideDuration: 2500 });
    }

    const download = async () => {
        if (!fileId) {
            enqueueSnackbar('Please enter a file code.');
            return;
        }

        cancelTokenSource = http.CancelToken.source();

        startLoading();

        await demoService.downloadFile(fileId, cancelTokenSource.token)
            .then((e) => onDownload(e))
            .catch((e) => onError(e))
            .finally(() => stopLoading());
    }

    const onDownload = (e: FileDownloadResult) => {
        enqueueSnackbar(`Downloaded ${e.name} from ${e.downloadedFrom}`, snackbarOptions);
    }

    const onError = (e: any) => {
        if (!(e instanceof String)) {
            e = 'An unknown error has occurred.';
        }

        enqueueSnackbar(e, snackbarOptions);
    }

    return (
        <div className='backup-demo'>
            <Typography variant='h1'>Backup demo</Typography>
            <div className='backup-demo__upload'>
                <Typography variant='h2'>Upload file</Typography>
                <p>
                    Get a taste of the performance and security Cloud Data Protection offers. Upload a file to get started.
                </p>
                <form onSubmit={(e) => onSubmit(e)}>
                    <div>
                        <FormLabel component='legend'>File</FormLabel>
                        <Button className='backup-demo__upload__form__select-file' variant='contained' component='label'>
                            {selectedFile ?
                                <span>{selectedFile.name} ({formatBytes(selectedFile.size)})</span> :
                                <span>Select a file</span>
                            }
                            <input type="file" hidden onChange={(e) => onFileSelect(e)} />
                        </Button>
                    </div>

                    <FormGroup className='backup-demo__sources'>
                        <FormLabel component='legend'>Backup destination</FormLabel>
                        {sources ? sources.sources.map((s) =>
                            <FormControlLabel
                                control={<Checkbox checked={isDestinationChecked(s)} name={s.fileDestination.toString()} />}
                                onChange={onDestinationChange}
                                label={s.description}
                                key={s.fileDestination.toString()}
                            />
                        ) :
                            <div>
                                <div className='skeleton skeleton--checkbox skeleton--16' />
                                <div className='skeleton skeleton--checkbox skeleton--16' />
                                <div className='skeleton skeleton--checkbox skeleton--16' />
                            </div>
                        }
                    </FormGroup>

                    {uploadedFile && uploadedFile.success &&
                        <div className='backup-demo__uploaded-file'>
                            Your file has been uploaded to {uploadedTo()}. You can access it later by saving the following code (click to copy): <code className='backup-demo__uploaded-file__id' onClick={(e) => copyToClipboard(e)}>
                                {uploadedFile.id}
                            </code>.
                            {uploadedFile.hasErrors &&
                                <p>An error has occurred while uploading the file to {uploadedToError()}.</p>
                            }
                        </div>
                    }
                    {uploadedFile && !uploadedFile.success &&
                        <div className='backup-demo__uploaded-file'>An error has occurred while uploading the file to {uploadedToError()}.</div>
                    }

                    <div className='backup-demo__upload__btn-container'>
                        <Button className='backup-demo__form__submit' type='submit' color='primary' variant='contained' disabled={!canSubmit()}>
                            Upload file
                        </Button>
                    </div>
                </form>
            </div>
            <div className='backup-demo__retrieve'>
                <Typography variant='h2'>Retrieve file</Typography>
                <p>
                    Retrieve a file by entering a code in the text field below.
                </p>
                <Input className='backup-demo__retrieve__input' type="text" placeholder="Enter code" value={fileId} onChange={(e) => setFileId(e.target.value)}/>
                {fileInfo &&
                    <div className='backup-demo__retrieve__file-info'>
                        <Typography variant='h5'>File info</Typography>
                        <List className='onboarding__backup-config__list' dense={true}>
                            <ListItem>
                                <ListItemIcon>
                                    <Description />
                                </ListItemIcon>
                                <ListItemText>
                                    Name: {fileInfo.name}
                                </ListItemText>
                            </ListItem>
                            <ListItem>
                                <ListItemIcon>
                                    <Crop />
                                </ListItemIcon>
                                <ListItemText>
                                    Size: {formatBytes(fileInfo.bytes)}
                                </ListItemText>
                            </ListItem>
                            <ListItem>
                                <ListItemIcon>
                                    <Info />
                                </ListItemIcon>
                                <ListItemText>
                                    Type: {fileInfo.contentType}
                                </ListItemText>
                            </ListItem>
                            <ListItem>
                                <ListItemIcon>
                                    <CloudOutlined />
                                </ListItemIcon>
                                <ListItemText>
                                    Uploaded to: {fileInfo.uploadedTo.map(u => u.description).sort().join(', ')}
                                </ListItemText>
                            </ListItem>
                        </List>
                    </div>
                }
                <div className='backup-demo__retrieve__btn-container'>
                    <Button className='backup-demo__retrieve' color='primary' variant='contained' disabled={fileInfo === undefined} onClick={() => download()}>
                        Download file
                    </Button>
                </div>
            </div>
        </div>
    )

}

export default Demo;