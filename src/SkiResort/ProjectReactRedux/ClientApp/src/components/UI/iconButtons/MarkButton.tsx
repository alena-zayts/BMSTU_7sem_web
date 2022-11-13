import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './IconButton.module.css';
import markImg from '../../../assets/mark2.png';




const MarkButton: any = ({ children, ...props }: InferProps<typeof MarkButton.propTypes>) => {
    return (
        <div className={classes.icon_div}>
            <button {...props} className={classes.icon_button}>
                <img className={classes.icon_img} src={markImg} alt="markButton" />
            </button>
        </div>
    );
};

MarkButton.propTypes = {
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

export default MarkButton;