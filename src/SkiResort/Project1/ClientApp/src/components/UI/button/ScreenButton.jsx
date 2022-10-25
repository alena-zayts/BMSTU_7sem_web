import React from 'react';
import classes from './ScreenButton.module.css';

const ScreenButton = ({children, ...props}) => {
    return (
        <button {...props} className={classes.screenBtn}>
            {children}
        </button>
    );
};

export default ScreenButton;