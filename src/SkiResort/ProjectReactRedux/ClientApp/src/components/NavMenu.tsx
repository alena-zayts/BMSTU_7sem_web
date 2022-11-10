import * as React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Nav } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import classes from './NavMenu.module.css';
import panelButtonClasses from '../components/UI/panelButton/PanelButton.module.css'
import logoIcon from '../assets/logo_icon.png';
import * as Styles from '../styles/components'

export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };

    public render() {
        return (
    //        <Navbar className="navbar my-navbar" role="navigation" aria-label="main navigation">

    //            <div className="navbar-brand">
    //                <a className="navbar-item" href="/">
    //                    <figure className="image is-30x64">
    //                        <img src="src/assets/logo_icon.png"/>
    //                            </figure>
    //                    SkiResort
    //                </a>
    //            </div>

    //                <div className="navbar-end">
    //                    <div className="navbar-item">
    //                        <div className="buttons">
    //                            <a className="button my-button" href="/hotels">
    //                                All Hotels
    //                            </a>

    //                            <a className="button my-button" href="/registr">
    //                                <strong>Sign up</strong>
    //                            </a>

    //                            <a className="button my-button" href="/login">
    //                                Sign in
    //                            </a>
    //                        </div>
    //                    </div>
    //                </div>
    //        </Navbar>
    //    );
    //}

    //        <header className={classes.nav_header}>
    //            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
    //                <div>
    //                    <div>
    //                        <NavbarBrand tag={Link} to="/">SkiResort</NavbarBrand>
    //                        <NavbarToggler onClick={this.toggle} className="mr-2" />
    //                    </div>

    //                    <div>
    //                    {/*<Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>*/}
    //                        {/*<ul className="navbar-end flex-grow">*/}
    //                        <ul className="justify-content-end  flex-grow">
    //                            <NavItem>
    //                                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
    //                            </NavItem>
    //                            <NavItem>
    //                                <NavLink tag={Link} className="text-dark" to="/lifts">Lifts</NavLink>
    //                            </NavItem>
    //                            <NavItem>
    //                                <NavLink tag={Link} className="text-dark" to="/account">Account</NavLink>
    //                            </NavItem>
    //                        </ul>
    //                        {/*</Collapse>*/}
    //                    </div>
    //                </div>
    //            </Navbar>
    //        </header>
    //    );
    //}


    //        <Navbar>
    //            <Container>
    //                <NavbarBrand tag={Link} to="/">SkiResort</NavbarBrand>
    //                <NavbarToggler onClick={this.toggle} className="mr-2" />
    //                <Collapse className="justify-content-end">
    //                    <NavItem>
    //                                <NavLink tag={Link} className="text-dark" to="/account">Account</NavLink>
    //                            </NavItem>
    //                </Collapse>
    //            </Container>
    //        </Navbar>
    //    );
    //}

            <header className={classes.nav_header}>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/" style={{ background: "#DDEAFD", padding: "10px"}}>
                            <Styles.SmallText style={{ display: "inline-block", margin: "0px 10px 0px 0px" }}>
                                SkiResort
                            </Styles.SmallText>
                            <img style={{ width: "80px", height: "60px", display: "inline-block"  }} src={logoIcon} alt="logoIconHere" />{/*style={nbStyle.logo}*/}
                        </NavbarBrand>
                        <NavbarToggler onClick={this.toggle} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>

                            <Nav className="me-auto" navbar>
                                {/*<NavItem>*/}
                                {/*    <NavLink tag={Link} className={panelButtonClasses.panelButton} to="/">Home</NavLink>*/}
                                {/*</NavItem>*/}
                                <NavItem>
                                    <NavLink tag={Link} style={{ margin: "0 10px" }} className={panelButtonClasses.panelButton} to="/lifts">Lifts</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} style={{ margin: "0 10px" }} className={panelButtonClasses.panelButton} to="/account">Account</NavLink>
                                </NavItem>
                            </Nav >
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }

    //        <div>
    //            <Navbar>
    //                <NavbarBrand href="/">reactstrap</NavbarBrand>
    //                <NavbarToggler onClick={this.toggle} />
    //                <Collapse isOpen={this.state.isOpen} navbar>
    //                    <Nav className="me-auto" navbar>
    //                        <NavItem>
    //                            <NavLink href="/components/">Components</NavLink>
    //                        </NavItem>
    //                        <NavItem>
    //                            <NavLink href="https://github.com/reactstrap/reactstrap">
    //                                GitHub
    //                            </NavLink>
    //                        </NavItem>
    //                    </Nav>
    //                    <NavItem>
    //                        <NavLink href="https://github.com/reactstrap/reactstrap">
    //                            GitHub
    //                        </NavLink>
    //                    </NavItem>
    //                </Collapse>
    //            </Navbar>
    //        </div>
    //    );
    //}

//                <header className={classes.nav_header}>
//    <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
//        <Container>
//            <NavbarBrand tag={Link} to="/">SkiResort</NavbarBrand>
//            <NavbarToggler onClick={this.toggle} className="mr-2" />
//            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
//                <ul className="navbar-nav flex-grow">
//                    <NavItem>
//                        <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
//                    </NavItem>
//                    <NavItem>
//                        <NavLink tag={Link} className="text-dark" to="/lifts">Lifts</NavLink>
//                    </NavItem>
//                    <NavItem>
//                        <NavLink tag={Link} className="text-dark" to="/account">Account</NavLink>
//                    </NavItem>
//                </ul>
//            </Collapse>
//        </Container>
//    </Navbar>
//</header>
//        );
//    }
    private toggle = () => {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
}
