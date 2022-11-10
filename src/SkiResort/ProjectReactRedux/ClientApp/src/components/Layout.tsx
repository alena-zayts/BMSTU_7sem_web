import * as React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import GlobalStyles from '../styles/global'

export default class Layout extends React.PureComponent<{}, { children?: React.ReactNode }> {
    public render() {
        return (
            /*<GlobalStyles />*/
            <React.Fragment>
                
                <NavMenu />

                {this.props.children}

                {/*<div>*/}
                {/*    {this.props.children}*/}
                {/*</div>*/}

                {/*<Container>* вылезет вправо/} 
                {/*    {this.props.children}*/}
                {/*</Container>*/}

            </React.Fragment>
        );
    }
}