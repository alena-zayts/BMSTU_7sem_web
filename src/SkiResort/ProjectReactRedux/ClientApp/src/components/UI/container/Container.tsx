import React from 'react';
import classes from './Container.module.css';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";

const Container: any = ({ children }: InferProps<typeof Container.propTypes>) => {
    return (
        <div className={classes.container}>
            {children}
         </div>
    );
};

Container.propTypes = {
    children: PropTypes.oneOfType([
        PropTypes.arrayOf(PropTypes.node),
        PropTypes.node
    ]),
};


export default Container;