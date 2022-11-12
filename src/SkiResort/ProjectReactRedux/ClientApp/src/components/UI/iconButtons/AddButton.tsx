import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './IconButton.module.css';
import addImg from '../../../assets/add.png';




const AddButton: any = ({ children, ...props }: InferProps<typeof AddButton.propTypes>) => {
    return (
        <div className={classes.icon_div}>
            <button {...props} className={classes.icon_button}>
                <img className={classes.icon_img} src={addImg} alt="addButton" />
            </button>
        </div>
    );
};

AddButton.propTypes = {
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

export default AddButton;