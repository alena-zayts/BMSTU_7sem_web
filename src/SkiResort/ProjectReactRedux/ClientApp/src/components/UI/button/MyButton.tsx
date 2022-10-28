import React from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './MyButton.module.css';

const MyButton: any = ({ children, ...props }) => {
    return (
        <button {...props} className={classes.myBtn}>
            {children}
        </button>
    );
};

export default MyButton;