import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import { AccountProps } from './AccountProps'
import AccountLogIn from './AccountLogIn';
import AccountProfile from './AccountProfile';
import { Redirect } from 'react-router-dom'


class Account extends React.PureComponent<AccountProps> {

    render() {
        if (this.props.userToken == undefined) {
            return <Redirect to='/account/login' />
        } else {
            return <Redirect to='/account/profile' />
        }
    }
};

export default connect(
  (state: ApplicationState) => state.user,
  UserStore.actionCreators
)(Account as any);
