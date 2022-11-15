import * as React from 'react';
import { Route, Switch } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';

import './custom.css'
import AccountLogIn from './components/Account/AccountLogIn';
import AccountProfile from './components/Account/AccountProfile';
import AccountRegister from './components/Account/AccountRegister';
import Account from './components/Account/Account';
import { ThemeProvider } from 'styled-components';
import { baseTheme } from './styles/theme';
import Lifts from './components/Lifts/Lifts';
import Messages from './components/Messages/Messages';
import PageNotFound from './components/PageNotFound';

export default () => (
    <ThemeProvider theme={baseTheme}>

        <Layout>
            {/*<Switch>*/}
                <Route exact path='/' component={Home} />

                <Route path='/lifts' component={Lifts} />
                <Route path='/users-messages' component={Messages} />

                

            <Route path='/account' component={Account} />
            <Route path='/account/login' component={AccountLogIn} />
            <Route path='/account/profile' component={AccountProfile} />
                <Route path='/account/register' component={AccountRegister} />


                {/*<Route component={PageNotFound} />*/}
            {/*</Switch>*/}
        </Layout>
    </ThemeProvider>
);
