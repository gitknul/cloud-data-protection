import {OptionsObject} from "notistack";

class SnackbarOptions {
    public static readonly base: OptionsObject = {
        autoHideDuration: 5000, anchorOrigin: {vertical: 'bottom', horizontal: 'center'}
    }

    public static readonly error: OptionsObject = {
        variant: 'error',
        ...SnackbarOptions.base
    }

    public static readonly success: OptionsObject = {
        variant: 'success',
        ...SnackbarOptions.base
    }

    public static readonly warning: OptionsObject = {
        variant: 'warning',
        ...SnackbarOptions.base
    }

    public static readonly info: OptionsObject = {
        variant: 'info',
        ...SnackbarOptions.base
    }
}

export default SnackbarOptions;