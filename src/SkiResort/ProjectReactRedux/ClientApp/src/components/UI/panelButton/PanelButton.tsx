import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './PanelButton.module.css';


const PanelButton: any = ({ children, ...props }: InferProps<typeof PanelButton.propTypes>) => {
    return (
        <button {...props} className={classes.panelButton}>
            {children}
        </button>
    );
};

PanelButton.propTypes = {
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

export default PanelButton;