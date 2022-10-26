import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import MyButton from '../UI/button/MyButton';
import { useSelector } from 'react-redux'
import { NavLink } from 'react-router-dom'

import * as Styles from '../../styles/components'
import { UnauthorizedAccount } from './UnauthorizedAccount';

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

                <UnauthorizedAccount />
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
