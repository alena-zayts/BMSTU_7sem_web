import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import LiftsList from './components/LiftsList';

import './custom.css'
import Account from './components/Account/Account';

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
        <Route path='/lifts' component={LiftsList} />
        <Route path='/account' component={Account} />
    </Layout>
);
