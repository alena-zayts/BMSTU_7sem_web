import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';

import './custom.css'
import AccountLogIn from './components/Account/AccountLogIn';
import AccountProfile from './components/Account/AccountProfile';
import AccountRegister from './components/Account/AccountRegister';
import Account from './components/Account/Account';
import { ThemeProvider } from 'styled-components';
import GlobalStyles from './styles/global'
import { baseTheme } from './styles/theme';
import Lifts from './components/Lifts/Lifts';

export default () => (
    <ThemeProvider theme={baseTheme}>

        <Layout>

            <Route exact path='/' component={Home} />

            <Route path='/lifts' component={Lifts} />

            <Route path='/account' component={Account} />
            <Route path='/account/login' component={AccountLogIn} />
            <Route path='/account/profile' component={AccountProfile} />
            <Route path='/account/register' component={AccountRegister} />
        </Layout>
    </ThemeProvider>
);
