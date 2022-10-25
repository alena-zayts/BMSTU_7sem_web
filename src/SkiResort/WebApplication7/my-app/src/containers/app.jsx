/*
 * Что тут интересного. Во-первых, мы объявили новый компонент строкой export default class App extends React.Component и экспортировали его, чтобы он был доступен извне.

Во-вторых, мы импортировали набор компонентов из пакета react-router-dom для организации клиентского роутинга. Router – это root компонент роутинга, в который должны быть вложены все остальные, а Switch и Route это сама организация роутинга. В данном случае мы хотим, чтобы при обращении к корневому пути (для примера — www.mytestsite.com) выводился наш компонент Blog, а при обращении к пути /about (www.mytestsite.com/about) выводился компонент About.

Header, About, Blog это наши пользовательские компоненты,
 */

import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Header from './header/header.jsx';
import About from './about/about.jsx';
import Blog from './blog/blog.jsx';

export default class App extends React.Component {
    render() {
        return (
            <Router>
                <div>
                    <Header />
                    <main>
                        <Switch>
                            <Route path="/about" component={About} />
                            <Route path="/" component={Blog} />
                        </Switch>
                    </main>
                </div>
            </Router>
        );
    }
};