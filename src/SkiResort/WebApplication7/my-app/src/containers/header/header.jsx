/*
 * навигация
 * Компонент будет генерировать ссылку, которая не будет отсылать вас на сервер, а 
 * будет изменять строку запроса в браузере, добавлять запись в историю, в общем вести себя как обычная ссылка, но без перезагрузки страницы.
*/

import React from 'react';
import { Link } from 'react-router-dom';

export default class Header extends React.Component {
    render() {
        return (
            <header>
                <menu>
                    <ul>
                        <li>
                            <Link to="/">Блог</Link>
                        </li>
                        <li>
                            <Link to="/about">Обо мне</Link>
                        </li>
                    </ul>
                </menu>
            </header>
        );
    }
};