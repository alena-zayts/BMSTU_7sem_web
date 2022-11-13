import * as React from 'react';
import { connect } from 'react-redux';
import classes from './App.module.css';
import * as Styles from '../styles/components'
import homeImg from '../assets/home_img700500.jpg';

const PageNorFound = () => (
    <div className={classes.main_div} >
        <div style={{ padding: "60px 0 0 0 " }}>
            <Styles.HeaderText>Page not found :(</Styles.HeaderText>
        </div>

        <p style={{ textAlign: "center" }}>
            <img src={homeImg} alt="homeImgHere" />
        </p>
    </div>
);

export default connect()(PageNorFound);
