import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './IconButton.module.css';
import deleteImg from '../../../assets/delete.png';




const DeleteButton: any = ({ children, ...props }: InferProps<typeof DeleteButton.propTypes>) => {
    return (
        <div className={classes.icon_div}>
            <button {...props} className={classes.icon_button}>
                <img className={classes.icon_img} src={deleteImg} alt="deleteButton" />
            </button>
        </div>
    );
};

DeleteButton.propTypes = {
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

export default DeleteButton;