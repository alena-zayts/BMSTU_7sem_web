import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import { AccountProps } from './AccountProps'
import { Redirect, withRouter } from 'react-router-dom'


class Account extends React.PureComponent<AccountProps> {

    render() {
        console.log('userToken from account: ' + this.props.userToken)
        if (this.props.userToken == undefined) {
            return <Redirect to='/account/login' />
        } else {
            return <Redirect to='/account/profile' />
        }
    }
};

export default withRouter(
    connect(
        (state: ApplicationState) => state.user,
        UserStore.actionCreators
)(Account as any));
