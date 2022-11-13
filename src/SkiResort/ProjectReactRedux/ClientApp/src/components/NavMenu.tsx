import React, { useState } from 'react';
import {
    Collapse,
    Navbar,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
} from 'reactstrap';
import logoIcon from '../assets/logo_icon.png';
import * as Styles from '../styles/components'
import HamburgerButton from './UI/iconButtons/HamburgerButton';
import AccountButton from './UI/iconButtons/AccountButton';
import panelButtonClasses from '../components/UI/panelButton/PanelButton.module.css'



export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };

    public render() {
        return (
            <div>
                <Navbar style={{ background: "#F5BF97", border: "2px solid #965437"}}>

                    <NavLink href="/account/" style={{ alignContent: "left", alignSelf: "left" }}>
                        <AccountButton/>
                    </NavLink>


                    <NavbarBrand style={{ background: "#DDEAFD", padding: "5px"  }} href="/">
                        <img alt="logoIcon" src={logoIcon} style={{height: 40, width: 40}}/>
                        <Styles.SmallText style={{ display: "inline-block", margin: "0px 0px 0px 10px" }}>
                            SkiResort
                        </Styles.SmallText>
                    </NavbarBrand>



                    <HamburgerButton onClick={this.toggle}/>
                    <Collapse isOpen={this.state.isOpen} navbar>
                        <Nav navbar>

                            <div style={{padding: "10px"}}>
                                <NavItem>
                                    <NavLink href="/lifts/" className={panelButtonClasses.panelButton} >Lifts</NavLink>
                                </NavItem>
                            </div>


                            {localStorage.getItem('userToken') != null &&
                                <div style={{ padding: "10px" }}>
                                    <NavItem>
                                        <NavLink href="/users-messages/" className={panelButtonClasses.panelButton}> Messages </NavLink>
                                    </NavItem>
                                </div>
                            }

                      
                        </Nav>
                    </Collapse>
                </Navbar>
            </div>
        );
    }

    private toggle = () => {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
}