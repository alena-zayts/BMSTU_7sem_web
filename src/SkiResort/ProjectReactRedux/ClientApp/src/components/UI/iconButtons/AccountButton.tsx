import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './IconButton.module.css';
import accountImg from '../../../assets/account.png';




const AccountButton: any = ({ children, ...props }: InferProps<typeof AccountButton.propTypes>) => {
    return (
        <div className={classes.icon_div}>
            <button {...props} className={classes.panel_icon_button}>
                <img className={classes.panel_icon_img} src={accountImg} alt="accountButton" />
            </button>
        </div>
    );
};

AccountButton.propTypes = {
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

export default AccountButton;