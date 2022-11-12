import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './IconButton.module.css';
import hamburgerImg from '../../../assets/hamburger.png';




const HamburgerButton: any = ({ children, ...props }: InferProps<typeof HamburgerButton.propTypes>) => {
    return (
        <div className={classes.icon_div}>
            <button {...props} className={classes.panel_icon_button}>
                <img className={classes.panel_icon_img} src={hamburgerImg} alt="hamburgerButton" />
            </button>
        </div>
    );
};

HamburgerButton.propTypes = {
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

export default HamburgerButton;