import * as React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import GlobalStyles from '../styles/global'

export default class Layout extends React.PureComponent<{}, { children?: React.ReactNode }> {
    public render() {
        return (
            <React.Fragment>
                
                <NavMenu />
                <GlobalStyles />
                <Container>
                    {this.props.children}
                </Container>
            </React.Fragment>
        );
    }
}