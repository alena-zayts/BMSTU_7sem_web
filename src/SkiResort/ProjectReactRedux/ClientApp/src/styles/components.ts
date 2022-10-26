import styled from 'styled-components'


//use <HeaderText>Some title H1</Title1>
export const HeaderText = styled.h1`
font-family: 'Cambay';
font-style: normal;
font-weight: 400;
font-size: 64px;
line-height: 104px;

color: #000000;
`

export const UsualText = styled.h1`
font-family: 'Cambay';
font-style: normal;
font-weight: 400;
font-size: 32px;
line-height: 52px;

color: #000000;
`


export const BoldUsualText = styled.h1`
font-family: 'Cambay';
font-style: normal;
font-weight: 700;
font-size: 32px;
line-height: 52px;

color: #000000;
`

// Пример заголовков разного уровня
//interface TitleProps {
//    weight?: 200 | 300 | 400 | 500 | 600 | 700
//}

//export const Title2 = styled.h2<TitleProps>`
//  font-size: 18px;
//  font-weight: ${({ weight = 700 }) => weight};
//`