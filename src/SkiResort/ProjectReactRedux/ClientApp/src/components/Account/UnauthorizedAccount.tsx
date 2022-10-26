import * as Styles from '../../styles/components';
import * as React from 'react';
import MyButton from '../UI/button/MyButton';

export const UnauthorizedAccount = () => {
    return (
                //        <div className='unauthorized'>
                //    <h1>Unauthorized :(</h1>
                //    <span>
                //        <NavLink to='/login'>Login</NavLink> to gain access
                //    </span>
                //</div>

        <React.Fragment>
            <Styles.HeaderText>Account (unauthorized)</Styles.HeaderText>
            <MyButton> LogIn </MyButton>
        </React.Fragment>
    );
}