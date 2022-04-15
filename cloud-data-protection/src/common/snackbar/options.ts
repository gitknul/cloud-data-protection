import {OptionsObject} from "notistack";

const base: OptionsObject = {
    autoHideDuration: 5000, anchorOrigin: {vertical: 'bottom', horizontal: 'center'}
}

const error: OptionsObject = {
    variant: 'error',
    ...base
}

const success: OptionsObject = {
    variant: 'success',
    ...base
}

const warning: OptionsObject = {
    variant: 'warning',
    ...base
}

const info: OptionsObject = {
    variant: 'info',
    ...base
}

const snackbarOptions = {
    error: error,
    success: success,
    warning: warning,
    info: info
}

export default snackbarOptions;