import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './MyButton.module.css';


const MyButton: any = ({ children, ...props }: InferProps<typeof MyButton.propTypes>) => {
    return (
        <button {...props} className={classes.myBtn}>
            {children}
        </button>
    );
};

MyButton.propTypes = {
    children: PropTypes.oneOfType([
        PropTypes.arrayOf(PropTypes.node),
        PropTypes.node
    ]),

    props: PropTypes.shape(
        {
            onChange: PropTypes.func.isRequired,
        }
    )
};

export default MyButton;