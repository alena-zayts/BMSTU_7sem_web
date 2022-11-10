import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './UsualButton.module.css';


const UsualButton: any = ({ children, ...props }: InferProps<typeof UsualButton.propTypes>) => {
    return (
        <button {...props} className={classes.myBtn}>
            {children}
        </button>
    );
};

UsualButton.propTypes = {
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

export default UsualButton;