import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import UsualButton from '../UI/usualButton/UsualButton';
//import Container from '../UI/container/Container';
import InputCell from '../UI/inputCell/InputCell';
import * as Styles from '../../styles/components'
import { AccountProps } from './AccountProps'
import { Redirect } from 'react-router-dom'
import MyLink from '../UI/link/MyLink';
import classes from '../App.module.css';
import buttonClasses from '../../components/UI/usualButton/UsualButton.module.css';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';


class AccountLogIn extends React.PureComponent<AccountProps, { userEmail: string, userPassword: string }> {
    constructor(props: AccountProps) {
        super(props);
        this.state = { userEmail: '', userPassword: '' };
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        this.props.logIn(this.state.userEmail, this.state.userPassword);
    }

    render() {
        if (this.props.userToken != undefined) {
            return <Redirect to='/account/profile' />
        }
        return (
            <div className={classes.main_div} >
                <Styles.HeaderText> LogIn </Styles.HeaderText>
                <InputCell
                    whatToInput="Email:"
                    value={this.state.userEmail}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, userEmail: e.target.value })}
                    type="text"
                />
                <InputCell
                    whatToInput="Password:"
                    value={this.state.userPassword}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, userPassword: e.target.value })}
                    type="text"
                />
                <UsualButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.handleSubmit(event)}>
                    LogIn
                </UsualButton>

                <div style={{ textAlign: "center", padding: "100px" }}>
                    {/*style={{ margin: "150px auto auto auto" }}*/}
                    <Styles.SmallText> Don't have account yet? </Styles.SmallText>
                    <NavLink tag={Link} to="/account/register">
                        <Styles.LinkText>Go to registration page</Styles.LinkText>
                    </NavLink>
                </div>
                {/*<NavLink tag={Link} style={{ margin: "0 10px" }} className={buttonClasses.myBtn} to="/account/register">Go to registration page</NavLink>*/}
                {/*<MyLink whereToNavigate={'/account/register'} linkText='Go to registration page' />*/}
                {/*<MyButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.redirectToRegster(event)}>*/}
                {/*    Go to registration page*/}
                {/*</MyButton>*/}
                {/*<Link className='btn btn-outline-secondary btn-sm' to={`/account/register`}>Register</Link>*/}
                
            </div>
        );
    }
}

export default connect(
  (state: ApplicationState) => state.user, // Selects which state properties are merged into the component's props
  UserStore.actionCreators // Selects which action creators are merged into the component's props
)(AccountLogIn as any); // eslint-disable-line @typescript-eslint/no-explicit-any
