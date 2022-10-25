//Ёто будет входной точкой в наше клиентское приложение. 
//¬кратце, в 2 верхних строчках мы импортируем все дл€ разработки на React, в 3 - й строке мы импортируем наш компонент - контейнер App, 
//который напишем ниже.¬ 4 - й строке рендерим этот компонент в DOM элемент с id =ФcontentФ.
//«апись вида < App /> это и есть JSX,

import React from 'react' //1
import { render } from 'react-dom' //2
import App from './containers/app.jsx' //3

render(
    <App />,
    document.getElementById('content')
) //4