import React, { ReactNode } from 'react';
import classes from './MyLink.module.css';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import { Link } from 'react-router-dom';

const MyLink: any = ({ children, ...props }: InferProps<typeof MyLink.propTypes>) => {
    return (
        <Link className={classes.myLink} to={props.whereToNavigate}>
            {props.linkText}
        </Link>
    );
};

MyLink.propTypes = {
    children: PropTypes.oneOfType([
        PropTypes.arrayOf(PropTypes.node),
        PropTypes.node
    ]),

    props: PropTypes.shape(
        {
            whereToNavigate: PropTypes.string.isRequired,
            linkText: PropTypes.string.isRequired,
        }
    )
};


export default MyLink;