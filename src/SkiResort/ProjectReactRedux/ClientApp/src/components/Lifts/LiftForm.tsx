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



class LiftForm extends React.PureComponent<LiftsProps, { liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number }> {
    constructor(props: LiftsProps) {
        super(props);
        this.state = { liftName: '', isOpen: false, seatsAmount: 0, liftingTime: 0 };
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        this.props.addLift(this.state.liftName, this.state.isOpen, this.state.seatsAmount, this.state.liftingTime);
    }

    render() {
        return (
            <Container>
                <Styles.HeaderText> Add Lift </Styles.HeaderText>
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
                <MyButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.handleSubmit(event)}>
                    Add
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




//const LiftForm: any = ({ children, ...props }: InferProps<typeof LiftForm.propTypes>) => {
//    return (
//        <button {...props} className={classes.myBtn}>
//            {children}
//        </button>
//    );
//};

//LiftForm.propTypes = {
//    children: PropTypes.oneOfType([
//        PropTypes.arrayOf(PropTypes.node),
//        PropTypes.node
//    ]),

//    props: PropTypes.shape(
//        {
//            onChange: PropTypes.func.isRequired,
//        }
//    )
//};

//export default LiftForm;