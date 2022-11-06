import * as React from 'react';
import { useState } from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as LiftsStore from '../../store/Lifts';
import { LiftsProps } from './LiftsProps'
import { Redirect } from 'react-router-dom'
import * as Styles from '../../styles/components'
import MyButton from '../UI/button/MyButton';
import LoadingScreen from '../UI/loadingScreen/LoadingScreen';
import MyTable from '../UI/table/MyTable';
import ModalWindow from '../UI/modalWindow/ModalWindow';
import LiftForm from './LiftForm';


class Lifts extends React.PureComponent<LiftsProps, { addLiftModalVisible: boolean }> {
    constructor(props: LiftsProps) {
        super(props);
        this.state = { addLiftModalVisible: false };
    }



    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.props.getLifts();
    }



    public render() {

        console.log(this.props)
        let liftsTable: any;
        if (this.props.isLoading == true) {
            liftsTable = (<LoadingScreen />);
        }
        else {
            liftsTable = this.renderLiftsTable();
        }

        return (
            <React.Fragment>
                <MyButton onClick={() => this.setState({ ...this.state, addLiftModalVisible: true })}>
                    Add Lift
                </MyButton>

                <ModalWindow visible={this.state.addLiftModalVisible} setVisible={(value: boolean) => this.setState({ ...this.state, addLiftModalVisible: value })}>
                    <LiftForm/>
                </ModalWindow>

                <Styles.HeaderText> Lifts </Styles.HeaderText>
                {liftsTable}
            </React.Fragment>
        );
    }

    private renderLiftsTable() {
        return <MyTable
            headNames={['liftID', 'liftName', 'isOpen', 'seatsAmount', 'liftingTime', 'queueTime']}
            rows={this.props.liftsList.map((lift: LiftsStore.Lift) =>
                [lift.LiftID,
                    lift.LiftName,
                    lift.IsOpen ? 'yes' : 'no',
                    lift.SeatsAmount,
                    lift.LiftingTime,
                    lift.QueueTime,
                    <MyButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.props.deleteLiftByName(lift.LiftName)}>
                        Delete
                    </MyButton>
                ]
                )} />
    }
}

export default connect(
    //(state: ApplicationState) => {state.lifts, state.user },
    (state: ApplicationState) => { return { ...state.user, ...state.lifts };},
    LiftsStore.actionCreators
)(Lifts as any);
