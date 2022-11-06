import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './ModalWindow.module.css';


const ModalWindow: any = ({ children, visible, setVisible }: InferProps<typeof ModalWindow.propTypes>) => {

    const rootClasses = [classes.modalWindow]

    if (visible) {
        rootClasses.push(classes.active);
    }

    return (
        <div className={rootClasses.join(' ')} onClick={() => setVisible(false)}>
            <div className={classes.modalWindowContent} onClick={(e) => e.stopPropagation()}>
                {children}
            </div>
        </div>
    );
};

ModalWindow.propTypes = {
    children: PropTypes.oneOfType([
        PropTypes.arrayOf(PropTypes.node),
        PropTypes.node
    ]),
    visible: PropTypes.bool.isRequired,
    setVisible: PropTypes.func.isRequired
};

export default ModalWindow;



