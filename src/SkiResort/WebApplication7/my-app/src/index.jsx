//��� ����� ������� ������ � ���� ���������� ����������. 
//�������, � 2 ������� �������� �� ����������� ��� ��� ���������� �� React, � 3 - � ������ �� ����������� ��� ��������� - ��������� App, 
//������� ������� ����.� 4 - � ������ �������� ���� ��������� � DOM ������� � id =�content�.
//������ ���� < App /> ��� � ���� JSX,

import React from 'react' //1
import { render } from 'react-dom' //2
import App from './containers/app.jsx' //3

render(
    <App />,
    document.getElementById('content')
) //4