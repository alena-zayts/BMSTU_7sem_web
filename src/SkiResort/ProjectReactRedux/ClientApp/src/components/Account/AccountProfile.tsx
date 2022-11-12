import * as React from 'react';
import { connect } from 'react-redux';
import { Link, Redirect } from 'react-router-dom';
import { ApplicationState } from '../../store';
import * as UserStore from '../../store/User';
import UsualButton from '../UI/usualButton/UsualButton';
import OutputCell from '../UI/outputCell/OutputCell';
import * as Styles from '../../styles/components'
import { AccountProps } from './AccountProps'
import LoadingScreen from '../UI/loadingScreen/LoadingScreen';
import classes from '../App.module.css';


class AccountProfile extends React.PureComponent<AccountProps, { isDataLoading: boolean }> {
    constructor(props: AccountProps) {
        super(props);
        this.state = { isDataLoading: false };
        this.loadUserInfo = this.loadUserInfo.bind(this);
    }
    public componentDidMount() {
        this.loadUserInfo();
    }

    loadUserInfo() {
        if (this.props.userToken != undefined) {
            this.setState({ ...this.state, isDataLoading: true });
            this.props.getUserInfo();
            this.setState({ ...this.state, isDataLoading: false });
        }
    }

    render() {
        if (this.props.userToken == undefined) {
            return <Redirect to='/account/login' />
        }

        if (this.props.userInfo == undefined) {
            return <LoadingScreen />
            
        }
        return (
            <div className={classes.main_div} >
                <div style={{ padding: "60px 0 0 0" }}>
                    <Styles.HeaderText> Profile </Styles.HeaderText>
                </div>
                <div style={{ padding: "30px 0 0 0" }}>
                    <OutputCell
                        whatToOutput="Email:"
                        value={this.props.userInfo.UserEmail}
                    />
                    {/*<OutputCell*/}
                    {/*    whatToOutput="Password:"*/}
                    {/*    value={this.props.userInfo.Password}*/}
                    {/*/>*/}
                    <OutputCell
                        whatToOutput="Role:"
                        value={this.props.userInfo.Role}
                    />
                    <OutputCell
                        whatToOutput="CardID:"
                        value={this.props.userInfo.CardID}
                    />
                    <UsualButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.props.logOut()}>
                        LogOut
                    </UsualButton>
                </div>
            </div>
        );
    }
}

export default connect(
  (state: ApplicationState) => state.user, 
  UserStore.actionCreators
)(AccountProfile as any); 
