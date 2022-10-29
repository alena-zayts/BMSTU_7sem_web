import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import MyButton from '../UI/button/MyButton';
import Container from '../UI/container/Container';
import InputCell from '../UI/inputCell/InputCell';
import { NavLink } from 'react-router-dom'
import * as Styles from '../../styles/components'
import { useForm } from 'react-hook-form'
//import { useNavigate } from 'react-router-dom'
//import { Redirect } from "react-router-dom";
import { useHistory } from "react-router-dom";
import { useDispatch, useSelector } from 'react-redux';
import { useEffect } from 'react'
import { AccountProps } from './AccountProps'



class AccountLogIn extends React.PureComponent<AccountProps, { userEmail: string, userPassword: string }> {

    //// This method is called when the component is first added to the document
    //public componentDidMount() {
    //    this.props.logIn("q", "q");
    //}

    constructor(props: AccountProps) {
        super(props);
        this.state = { userEmail: '', userPassword: '' };
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        try {
            this.props.logIn(this.state.userEmail, this.state.userPassword);
            console.log('here');
            console.log(this.props.userToken);
        }
        catch (e) {
            
            const result = e.message; // error under useUnknownInCatchVariables 
            if (typeof e === "string") {
                e.toUpperCase() // works, `e` narrowed to string
            } else if (e instanceof Error) {
                e.message // works, `e` narrowed to Error
            }
            alert(result)
        }
        event.preventDefault();
    }

    render() {
        console.log('rendering');
        console.log(this.props)
        console.log(this.props.userToken);
        console.log(this.props.userInfo);
        if (this.props.userToken) {
            return <Styles.HeaderText> Account LogIn with token </Styles.HeaderText>
        }
        return (
            <Container>
                <Styles.HeaderText> Profile </Styles.HeaderText>
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
                <MyButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.handleSubmit(event)}>
                    LogIn
                </MyButton>
            </Container>
        );
    }

        //const navigate = useNavigate()
    //const history = UseHistory();

    // redirect authenticated user to profile screen
    //useEffect(() => {
    //    if (userInfo) {
    //        //return new Redirect("/account");
    //        //history.pushState('/your-component')
    //        navigate('/account')
    //    }
    //}, [navigate, userInfo])
}

export default connect(
  (state: ApplicationState) => state.user, // Selects which state properties are merged into the component's props
  UserStore.actionCreators // Selects which action creators are merged into the component's props
)(AccountLogIn as any); // eslint-disable-line @typescript-eslint/no-explicit-any
