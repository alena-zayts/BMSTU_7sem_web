import React, { ReactNode } from 'react';
import * as Styles from '../../styles/components'
import { connect } from 'react-redux';
import UsualButton from '../UI/usualButton/UsualButton';
import InputCell from '../UI/inputCell/InputCell';
import { ApplicationState } from '../../store';
import * as LiftsStore from '../../store/Lifts';
import classes from '../UI/modalWindow/ModalWindow.module.css';

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




class LiftForm extends React.PureComponent<LiftFormProps,
    { liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number, connectedSlopeNames: string }>
{
    constructor(props: LiftFormProps) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.state = {
            liftName: props.liftName.valueOf(), isOpen: props.isOpen, seatsAmount: props.seatsAmount,
            liftingTime: props.liftingTime, connectedSlopeNames: props.connectedSlopeNames
        };

    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        this.props.buttonAction(this.state.liftName, this.state.isOpen, this.state.seatsAmount, this.state.liftingTime, this.state.connectedSlopeNames);
    }

    render() {
        return (
            <div className={classes.modalWindowDiv} >
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
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, isOpen: e.target.value as any as boolean })}
                    type="boolean"
                />
                <InputCell
                    whatToInput="seatsAmount:"
                    value={this.state.seatsAmount}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, seatsAmount: e.target.value as any as number })}
                    type="number"
                />
                <InputCell
                    whatToInput="liftingTime:"
                    value={this.state.liftingTime}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, liftingTime: e.target.value as any as number })}
                    type="number"
                />
                <InputCell
                    whatToInput="slopes:"
                    value={this.state.connectedSlopeNames}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, connectedSlopeNames: e.target.value })}
                    type="text"
                />
                <UsualButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.handleSubmit(event)}>
                    {this.props.buttonText}
                </UsualButton>
            </div>
        );
    }
}
export default connect(
    (state: ApplicationState) => state.lifts,
    LiftsStore.actionCreators
)(LiftForm as any);