import React, { ReactNode } from 'react';
import * as Styles from '../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './MyButton.module.css';
import { connect } from 'react-redux';
import MyButton from '../UI/button/MyButton';
import Container from '../UI/container/Container';
import InputCell from '../UI/inputCell/InputCell';
import { Redirect } from 'react-router-dom'
import MyLink from '../UI/link/MyLink';
import { LiftsProps } from './LiftsProps'
import { RouteComponentProps } from 'react-router';
import * as UserStore from '../../store/User';
import { ApplicationState } from '../../store';
import * as LiftsStore from '../../store/Lifts';

interface LiftFormProps {
    liftName: string,
    isOpen: boolean,
    seatsAmount: number,
    liftingTime: number,
    connectedSlopeNames: string,
    formTitle: string,
    buttonText: string,
    buttonAction: (
        liftName: string,
        isOpen: boolean,
        seatsAmount: number,
        liftingTime: number,
        connectedSlopeNames: string,
    ) => void
}




class LiftForm extends React.PureComponent<LiftFormProps, { liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number, connectedSlopeNames: string}> {
    //constructor(props: LiftsProps, other: { buttonText: string, buttonHandler: Function, isOpen: boolean = true, liftName: string = "123", seatsAmount: number = 0, liftingTime: number = 0, connectedSlopeNames: string[] = [] }) {
    constructor(props: LiftFormProps) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.state = { liftName: props.liftName.valueOf(), isOpen: props.isOpen, seatsAmount: props.seatsAmount, liftingTime: props.liftingTime, connectedSlopeNames: props.connectedSlopeNames };

    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        this.props.buttonAction(this.state.liftName, this.state.isOpen, this.state.seatsAmount, this.state.liftingTime, this.state.connectedSlopeNames);
    }

    render() {
        return (
            <Container>
                <Styles.HeaderText> {this.props.formTitle} </Styles.HeaderText>
                <InputCell
                    whatToInput="liftName:"
                    value={this.state.liftName}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, liftName: e.target.value })}
                    type="text"
                />
                <InputCell
                    whatToInput="isOpen:"
                    value={this.state.isOpen}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, isOpen: e.target.value })}
                    type="boolean"
                />
                <InputCell
                    whatToInput="seatsAmount:"
                    value={this.state.seatsAmount}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, seatsAmount: e.target.value })}
                    type="number"
                />
                <InputCell
                    whatToInput="liftingTime:"
                    value={this.state.liftingTime}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, liftingTime: e.target.value })}
                    type="number"
                />
                <InputCell
                    whatToInput="slopes:"
                    value={this.state.connectedSlopeNames}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, connectedSlopeNames: e.target.value })}
                    type="text"
                />
                <MyButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.handleSubmit(event)}>
                    {this.props.buttonText}
                </MyButton>
            </Container>
        );
    }
}
export default connect(
    //(state: ApplicationState) => {state.lifts, state.user },
    (state: ApplicationState) => state.lifts,
    LiftsStore.actionCreators
)(LiftForm as any);