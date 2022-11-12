import * as React from 'react';
import { connect } from 'react-redux';
import classes from './App.module.css';
import * as Styles from '../styles/components'
import homeImg from '../assets/home_img700500.jpg';

const Home = () => (
    <div className={classes.main_div} >
        <div style={{padding: "60px 0 0 0 "}}>
            <Styles.HeaderText>Welcome to SkiResort!</Styles.HeaderText>
        </div>
        <div style={{ padding: "30px 0 20px 0 ", textAlign: "center" }}>
            <Styles.UsualText>
                You rush along the powder, and snow dust flies in your face,
                <br />
                burning and delighting. An indescribable feeling!
            </Styles.UsualText>
        </div>

        <p style={{ textAlign: "center"}}>
            <img src={homeImg} alt="homeImgHere" />
        </p>
    </div>
);

export default connect()(Home);
