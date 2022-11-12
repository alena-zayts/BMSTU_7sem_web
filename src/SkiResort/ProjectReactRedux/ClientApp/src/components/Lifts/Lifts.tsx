import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as LiftsStore from '../../store/Lifts';
import * as UserStore from '../../store/User';
import * as Styles from '../../styles/components'
import UsualButton from '../UI/usualButton/UsualButton';
import LoadingScreen from '../UI/loadingScreen/LoadingScreen';
import MyTable from '../UI/table/MyTable';
import ModalWindow from '../UI/modalWindow/ModalWindow';
import LiftForm from './LiftForm';
import classes from '../App.module.css';
import DeleteButton from '../UI/iconButtons/DeleteButton';
import UpdateButton from '../UI/iconButtons/UpdateButton';
import InputCell from '../UI/inputCell/InputCell';
import { LiftsProps } from './LiftsProps'



class Lifts extends React.PureComponent<LiftsProps, {searchLiftName: string, updateModalWindow: any, addLiftModalVisible: boolean, updateLiftModalVisible: boolean, currentliftName: string, currentisOpen: boolean, currentseatsAmount: number, currentliftingTime: number, currentconnectedSlopeNames: string }>
{
    constructor(props: LiftsProps) {
        super(props);
        this.state = {
            searchLiftName: "", updateModalWindow: null, addLiftModalVisible: false, updateLiftModalVisible: false,
            currentliftName: "", currentisOpen: false, currentseatsAmount: 0, currentliftingTime: 0, currentconnectedSlopeNames: ""
        };
        this.processAddAction = this.processAddAction.bind(this);
        this.processUpdateAction = this.processUpdateAction.bind(this);
    }

    // This method is called when the component is first added to the document
    public componentDidMount() {
        if (!this.props.Role) {
            this.props.getUserInfo();
        }
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
        this.setState({ ...this.state, addLiftModalVisible: false })
    }

    processUpdateAction(liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number, connectedSlopeNames: string) {
        this.props.updateLift(liftName, isOpen, seatsAmount, liftingTime, this.convertStrToArray(connectedSlopeNames))
        this.setState({ ...this.state, updateLiftModalVisible: false })
    }

    addActionAvailable(): boolean {
        return this.props.userInfo != undefined && this.props.userInfo.Role == "admin"
    }

    deleteActionAvailable(): boolean {
        return this.props.userInfo != undefined && this.props.userInfo.Role == "admin"
    }

    updateActionAvailable(): boolean {
        return this.props.userInfo != undefined && (this.props.userInfo.Role == "admin" || this.props.userInfo.Role == "ski_patrol")
    }

    actionsAvailable() {
        return this.updateActionAvailable() || this.deleteActionAvailable()
    }

    public render() {
        let liftsTable: any;
        if (this.props.isLoading == true) {
            liftsTable = (<LoadingScreen />);
        }
        else {
            const filteredLiftsList = this.props.liftsList.filter(
                lift => lift.LiftName.toLowerCase().includes(this.state.searchLiftName.toLowerCase())
            )
            liftsTable = this.renderLiftsTable(filteredLiftsList);
        }
        
        return (
            <div className={classes.main_div} >

                <ModalWindow visible={this.state.addLiftModalVisible} setVisible={(value: boolean) => this.setState({ ...this.state, addLiftModalVisible: value })}>
                    <LiftForm formTitle="Add Lift" liftName="" isOpen={false} seatsAmount={0} liftingTime={0} connectedSlopeNames="" buttonText="Add" buttonAction={this.processAddAction}/>
                </ModalWindow>

                {this.state.updateModalWindow != null && this.state.updateLiftModalVisible && this.state.updateModalWindow}
                <Styles.HeaderText>
                    Lifts
                </Styles.HeaderText>

                {this.addActionAvailable() &&
                    <UsualButton onClick={() => this.setState({ ...this.state, addLiftModalVisible: true })}>
                        Add Lift
                    </UsualButton>
                }

                <InputCell
                    whatToInput="Find by liftName:"
                    value={this.state.searchLiftName}
                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => this.setState({ searchLiftName: event.target.value })}
                    type="text"
                />

                {liftsTable}
            </div>
        );
    }

    private renderLiftsTable(lifts: LiftsStore.Lift[]) {
        return <MyTable
            headNames={['liftID', 'liftName', 'isOpen', 'seatsAmount', 'liftingTime', 'queueTime', 'slopes', this.actionsAvailable() ? 'actions' : '']}
            rows={lifts.map((lift: LiftsStore.Lift) =>
                [lift.LiftID,
                    lift.LiftName,
                    lift.IsOpen ? 'yes' : 'no',
                    lift.SeatsAmount,
                    lift.LiftingTime,
                    lift.QueueTime,
                    lift.ConnectedSlopeNames ? lift.ConnectedSlopeNames.join(" ") : "",

                    <div style={{ display: 'flex', margin: "0 -5px" }}>
                        <div style={{ flex: "1 1 auto", margin: "0 5px" }}>
                            {this.deleteActionAvailable() &&
                                <DeleteButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.props.deleteLiftByName(lift.LiftName)} />
                            }
                        </div>

                        <div style={{ flex: "1 1 auto", margin: "0 5px" }}>
                            {this.updateActionAvailable() &&
                                <UpdateButton style={{ flex: "1 1 auto", margin: "0 5px" }} onClick={(event: React.FormEvent<HTMLFormElement>) => {
                                    this.setState({
                                        ...this.state,
                                        updateLiftModalVisible: true,
                                        updateModalWindow:
                                            <ModalWindow visible={true} setVisible={(value: boolean) => this.setState({
                                                ...this.state,
                                                updateLiftModalVisible: value
                                            })}>
                                                <LiftForm formTitle="Update Lift" liftName={lift.LiftName} isOpen={lift.IsOpen} seatsAmount={lift.SeatsAmount}
                                                    liftingTime={lift.LiftingTime}
                                                    connectedSlopeNames={lift.ConnectedSlopeNames ? lift.ConnectedSlopeNames.join(" ") : ""}
                                                    buttonText="Update" buttonAction={this.processUpdateAction} />
                                            </ModalWindow>
                                    })
                                }} />
                            }
                        </div>
                    </div>
                ]
                )} />
    }
}



const mapStateToProps = (state: ApplicationState) => ({
    ...state.user,
    ...state.lifts
});

const mapDispatchToProps = ({
    ...UserStore.actionCreators,
    ...LiftsStore.actionCreators
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Lifts as any);
