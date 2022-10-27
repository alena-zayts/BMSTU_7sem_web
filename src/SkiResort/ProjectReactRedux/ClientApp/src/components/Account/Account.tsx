import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import MyButton from '../UI/button/MyButton';
import { NavLink } from 'react-router-dom'
import * as Styles from '../../styles/components'
import { useForm } from 'react-hook-form'
//import { useNavigate } from 'react-router-dom'
//import { Redirect } from "react-router-dom";
import { useHistory } from "react-router-dom";
import { useDispatch, useSelector } from 'react-redux';
import { useEffect } from 'react'

// At runtime, Redux will merge together...
type AccountProps =
    UserStore.UserState // ... state we've requested from the Redux store
    & typeof UserStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{}>; // ... plus incoming routing parameters


class Account extends React.PureComponent<AccountProps> {

    //// This method is called when the component is first added to the document
    //public componentDidMount() {
    //    this.props.logIn("q", "q");
    //}

    public render() {
        // show unauthorized screen if no user is found in redux store
        if (this.props.userInfo == null || this.props.userInfo.Role == "unathurozied") //admin, ski_patrol, authorized
        {
            return (

                <NameForm {...this.props} />
            )
        }
    return (
      <React.Fragment>
            <Styles.HeaderText> Profile (default) </Styles.HeaderText>
            <p> {this.props.userToken} </p>
      </React.Fragment>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.user, // Selects which state properties are merged into the component's props
  UserStore.actionCreators // Selects which action creators are merged into the component's props
)(Account as any); // eslint-disable-line @typescript-eslint/no-explicit-any




class NameForm extends React.Component<AccountProps, { userEmail: string, userPassword: string }> {
    constructor(props) {
        super(props);
        this.state = { userEmail: '', userPassword: ''};
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event: React.ChangeEvent<HTMLInputElement>) {
        const target = event.target;
        const value = target.value;
        const name = target.name;

        switch (name) {
            case 'userEmail':
                this.setState({ userEmail: value })
                break;
            case 'userPassword':
                this.setState({ userPassword: value })
                break;
        }
        alert('Info: ' + value + name);

        //this.setState({
        //    [name]: value
        //});
    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        alert('Info: ' + this.state.userEmail + this.state.userPassword);
        try {
            
            this.props.logIn(this.state.userEmail, this.state.userPassword);
            //const dispatch = useDispatch();
            //dispatch(this.props.logIn(this.state.userEmail, this.state.userPassword))
        }
        catch (e)
        {
            const result = e.message; // error under useUnknownInCatchVariables 
            if (typeof e === "string") {
                e.toUpperCase() // works, `e` narrowed to string
            } else if (e instanceof Error) {
                e.message // works, `e` narrowed to Error
            }
            console.log(result)
        }
        event.preventDefault();
        //event.preventDefault();
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <label>
                    Email
                    <input name="userEmail" type="text" value={this.state.userEmail} onChange={this.handleChange} />
                </label>
                <label>
                    Password
                    <input name="userPassword" type="text" value={this.state.userPassword} onChange={this.handleChange} />
                </label>
                <input type="submit" value="Отправить" />
            </form>
        );
    }
}

connect(
    (state: ApplicationState) => state.user, // Selects which state properties are merged into the component's props
    UserStore.actionCreators // Selects which action creators are merged into the component's props
)(NameForm as any); // eslint-disable-line @typescript-eslint/no-explicit-any


// нужен AccountProps
const UnauthorizedAccount = () => {
    const loading = useSelector((state: ApplicationState) => state.user.loading);
    const userInfo = useSelector((state: ApplicationState) => state.user.userInfo);
    const error = useSelector((state: ApplicationState) => state.user.error);
    const dispatch = useDispatch()
    const { register, handleSubmit } = useForm()
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

    const submitForm = (data: { userEmail: string, userPassword: string }) => {
        dispatch(UserStore.actionCreators.logIn(data.userEmail, data.userPassword))
    }

    return (
        <form onSubmit={handleSubmit(submitForm)}>
            <div className='form-group'>
                <label htmlFor='email'>Email</label>
                <input
                    type='email'
                    className='form-input'
                    {...register('userEmail')}
                    required
                />
            </div>
            <div className='form-group'>
                <label htmlFor='password'>Password</label>
                <input
                    type='password'
                    className='form-input'
                    {...register('userPassword')}
                    required
                />
            </div>
            <button type='submit' className='button' disabled={loading}>
                Login
            </button>
        </form>
    )
}