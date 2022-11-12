import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import UsualButton from '../UI/usualButton/UsualButton';
import InputCell from '../UI/inputCell/InputCell';
import * as Styles from '../../styles/components'
import { AccountProps } from './AccountProps'
import { Redirect } from 'react-router-dom'
import classes from '../App.module.css';
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
                <div style={{ padding: "60px 0 0 0" }}>
                    <Styles.HeaderText> LogIn </Styles.HeaderText>
                </div>
                <div style={{ padding: "30px 0 0 0" }}>
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
                </div>
                <div style={{ textAlign: "center", padding: "100px" }}>
                    <Styles.SmallText> Don't have account yet? </Styles.SmallText>
                    <NavLink tag={Link} to="/account/register">
                        <Styles.LinkText>Go to registration page</Styles.LinkText>
                    </NavLink>
                </div>
            </div>
        );
    }
}

export default connect(
  (state: ApplicationState) => state.user,
  UserStore.actionCreators
)(AccountLogIn as any); 
