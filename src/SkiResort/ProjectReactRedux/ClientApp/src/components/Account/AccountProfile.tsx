import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import MyButton from '../UI/button/MyButton';
import Container from '../UI/container/Container';
import OutputCell from '../UI/outputCell/OutputCell';
import { NavLink } from 'react-router-dom'
import * as Styles from '../../styles/components'
import { useForm } from 'react-hook-form'
//import { useNavigate } from 'react-router-dom'
//import { Redirect } from "react-router-dom";
import { useHistory } from "react-router-dom";
import { useDispatch, useSelector } from 'react-redux';
import { useEffect } from 'react'
import { AccountProps } from './AccountProps'
import { getUserInfoByTocken } from '../../actions/UserActions';
//import { dispatch } from '../../../store';
import { getUser } from './action/user';



class AccountProfile extends React.PureComponent<AccountProps, {}> {
    //loadData() {
    //    // from here, store's state should be { fetching: true, data: [] }
    //    dispatch(getUserInfoByTocken())
    //        .then((userData) => {
    //            console.log('userData in AccountProfile')
    //            console.log(userData)
    //            // now from here, you can get the value from parameter or you can get it from store or component props if super component passing it by redux provider.
    //            // store state should be { fetching: false: data [..., ...] }
    //            // do something with received data
    //        })
    //        .catch((error) => {
    //            console.log('error in AccountProfile')
    //            console.log(error)
    //        })
    //};

    render() {
        //this.loadData()
        if (this.props.userInfo == null) {
            console.log(this.props.userToken)
            return (<Styles.HeaderText> Profile (2) not log in </Styles.HeaderText>)
        }
        return (
            <Container>
                <Styles.HeaderText> Profile (2) </Styles.HeaderText>
                <OutputCell
                    whatToOutput="Email:"
                    value={this.props.userInfo.UserEmail}
                />
                <OutputCell
                    whatToOutput="Password:"
                    value={this.props.userInfo.Password}
                />
                <OutputCell
                    whatToOutput="Role:"
                    value={this.props.userInfo.Role}
                />
                <OutputCell
                    whatToOutput="CardID:"
                    value={this.props.userInfo.CardID}
                />
                {/*<MyButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.handleSubmit(event)}>*/}
                {/*    LogIn*/}
                {/*</MyButton>*/}
            </Container>
        );
    }
}

export default connect(
  (state: ApplicationState) => state.user, // Selects which state properties are merged into the component's props
  UserStore.actionCreators // Selects which action creators are merged into the component's props
)(AccountProfile as any); // eslint-disable-line @typescript-eslint/no-explicit-any
