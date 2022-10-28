import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import LiftsList from './components/LiftsList';

import './custom.css'
import Account from './components/Account/AccountLogIn';
import { ThemeProvider } from 'styled-components';
import GlobalStyles from './styles/global'
import { baseTheme } from './styles/theme';

export default () => (
    <ThemeProvider theme={baseTheme}>

        <Layout>

        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
        <Route path='/lifts' component={LiftsList} />
        <Route path='/account/login' component={Account} />
        </Layout>
    </ThemeProvider>
);
