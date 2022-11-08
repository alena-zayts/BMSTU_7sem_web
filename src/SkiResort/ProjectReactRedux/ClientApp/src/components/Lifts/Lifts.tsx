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
import Container from '../UI/container/Container';


class Lifts extends React.PureComponent<LiftsProps, { updateModalWindow: any, addLiftModalVisible: boolean, updateLiftModalVisible: boolean, currentliftName: string, currentisOpen: boolean, currentseatsAmount: number, currentliftingTime: number, currentconnectedSlopeNames: string }> {
    constructor(props: LiftsProps) {
        super(props);
        this.state = { updateModalWindow: null, addLiftModalVisible: false, updateLiftModalVisible: false, currentliftName: "", currentisOpen: false, currentseatsAmount: 0, currentliftingTime: 0, currentconnectedSlopeNames: ""};
        this.processAddAction = this.processAddAction.bind(this);
        this.processUpdateAction = this.processUpdateAction.bind(this);
    }



    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.props.getLifts();
    }

    convertStrToArray(str: string) {
        let arr: string[] = str.split(' ')
        arr = arr.filter(function (item) {
            return item !== " " && item !== ""
        })
        return arr
    }
    processAddAction(liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number, connectedSlopeNames: string) {
        this.props.addLift(liftName, isOpen, seatsAmount, liftingTime, this.convertStrToArray(connectedSlopeNames))
    }

    processUpdateAction(liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number, connectedSlopeNames: string) {
        this.props.updateLift(liftName, isOpen, seatsAmount, liftingTime, this.convertStrToArray(connectedSlopeNames))
    }


    public render() {
        let liftsTable: any;
        if (this.props.isLoading == true) {
            liftsTable = (<LoadingScreen />);
        }
        else {
            liftsTable = this.renderLiftsTable();
        }

        return (
            <Container>
                <MyButton onClick={() => this.setState({ ...this.state, addLiftModalVisible: true })}>
                    Add Lift
                </MyButton>

                <ModalWindow visible={this.state.addLiftModalVisible} setVisible={(value: boolean) => this.setState({ ...this.state, addLiftModalVisible: value })}>
                    <LiftForm formTitle="Add Lift" liftName="" isOpen={false} seatsAmount={0} liftingTime={0} connectedSlopeNames="" buttonText="Add" buttonAction={this.processAddAction}/> //
                </ModalWindow>

                {this.state.updateModalWindow != null && this.state.updateLiftModalVisible && this.state.updateModalWindow}

                <Styles.HeaderText> Lifts </Styles.HeaderText>
                {liftsTable}
            </Container>
        );
    }

    private renderLiftsTable() {
        return <MyTable
            headNames={['liftID', 'liftName', 'isOpen', 'seatsAmount', 'liftingTime', 'queueTime', 'slopes']}
            rows={this.props.liftsList.map((lift: LiftsStore.Lift) =>
                [lift.LiftID,
                    lift.LiftName,
                    lift.IsOpen ? 'yes' : 'no',
                    lift.SeatsAmount,
                    lift.LiftingTime,
                    lift.QueueTime,
                    lift.ConnectedSlopeNames ? lift.ConnectedSlopeNames.join(" ") : "",
                    <div style={{display: 'flex', padding: "5px" }}>
                        <MyButton style={{ flex: '1 1 auto', margin: "0 5px" }}  onClick={(event: React.FormEvent<HTMLFormElement>) => this.props.deleteLiftByName(lift.LiftName)}>
                            Delete
                        </MyButton>

                        <MyButton style={{ flex: '1 1 auto', margin: "0 5px" }} onClick={(event: React.FormEvent<HTMLFormElement>) => {
                            this.setState({
                                ...this.state,
                                updateLiftModalVisible: true,
                                updateModalWindow:
                                    <ModalWindow visible={true} setVisible={(value: boolean) => this.setState({ ...this.state, updateLiftModalVisible: value })}>
                                        <LiftForm formTitle="Update Lift" liftName={lift.LiftName} isOpen={lift.IsOpen} seatsAmount={lift.SeatsAmount} liftingTime={lift.LiftingTime} connectedSlopeNames={lift.ConnectedSlopeNames ? lift.ConnectedSlopeNames.join(" ") : ""} buttonText="Update" buttonAction={this.processUpdateAction} />
                                    </ModalWindow>
                            })
                        }}>
                            Update
                        </MyButton>
                    </div>
                ]
                )} />
    }
}

export default connect(
    (state: ApplicationState) => { return { ...state.user, ...state.lifts };},
    LiftsStore.actionCreators
)(Lifts as any);
