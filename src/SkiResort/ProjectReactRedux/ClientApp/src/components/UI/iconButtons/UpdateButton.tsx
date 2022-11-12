import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './IconButton.module.css';
import updateImg from '../../../assets/update.png';




const UpdateButton: any = ({ children, ...props }: InferProps<typeof UpdateButton.propTypes>) => {
    return (
        <div className={classes.icon_div}>
            <button {...props} className={classes.icon_button}>
                <img className={classes.icon_img} src={updateImg} alt="updateButton" />
            </button>
        </div>
    );
};

UpdateButton.propTypes = {
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

export default UpdateButton;