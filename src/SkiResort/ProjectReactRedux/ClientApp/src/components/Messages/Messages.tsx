import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as MessagesStore from '../../store/Messages';
import * as UserStore from '../../store/User';
import * as Styles from '../../styles/components'
import UsualButton from '../UI/usualButton/UsualButton';
import LoadingScreen from '../UI/loadingScreen/LoadingScreen';
import MyTable from '../UI/table/MyTable';
import ModalWindow from '../UI/modalWindow/ModalWindow';
import MessageForm from './MessageForm';
import classes from '../App.module.css';
import classesModal from '../UI/modalWindow/ModalWindow.module.css';
import DeleteButton from '../UI/iconButtons/DeleteButton';
import UpdateButton from '../UI/iconButtons/UpdateButton';
import InputCell from '../UI/inputCell/InputCell';
import { MessagesProps } from './MessagesProps'
import MarkButton from '../UI/iconButtons/MarkButton';
import { Redirect, withRouter } from 'react-router-dom'



class Messages extends React.PureComponent<MessagesProps,
    {
    searchMessageID: number | null, updateModalWindow: any, updateMessageModalVisible: boolean,
    currentmessageID: number | null, currentCheckedByID: number | null, currentsenderID: number | null, currentText: string
    }>
{
    constructor(props: MessagesProps) {
        super(props);
        this.state = {
            searchMessageID: null, updateModalWindow: null, updateMessageModalVisible: false,
            currentmessageID: null, currentsenderID: null, currentCheckedByID: null, currentText: ""
        };
        this.processUpdateAction = this.processUpdateAction.bind(this);
        this.processMarkAction = this.processMarkAction.bind(this);
    }


    public componentDidMount() {
        
        if (this.props.userInfo == undefined) {
            this.props.getUserInfo();
        }
       
    }

    //public shouldComponentUpdate(nextProps: MessagesProps, nextState: any) {
    //    if (nextState.searchMessageID != this.state.searchMessageID)
    //        return false;
    //    return true
    //}

    processUpdateAction(messageID: number, senderID: number, checkedByID: number, text: string) {
        this.props.updateMessage(messageID, senderID, checkedByID, text)
        this.setState({ ...this.state, updateMessageModalVisible: false })
    }

    processMarkAction(messageID: number) {
        this.props.markMessageRead(messageID)
    }


    messagesAvailable(): boolean {
        return (this.props.userInfo != undefined) && (this.props.userInfo.Role == "admin" || this.props.userInfo.Role == "ski_patrol")
    }

    sendActionAvailable(): boolean {
        return (this.props.userInfo != undefined)
    }

    searchActionAvailable(): boolean {
        return (this.props.userInfo != undefined) && (this.props.userInfo.Role == "admin" || this.props.userInfo.Role == "ski_patrol")
    }

    deleteActionAvailable(): boolean {
        return (this.props.userInfo != undefined) && this.props.userInfo.Role == "admin"
    }

    updateActionAvailable(): boolean {
        return (this.props.userInfo != undefined) && this.props.userInfo.Role == "admin"
    }

    markCheckedActionAvailable(): boolean {
        return (this.props.userInfo != undefined) && this.props.userInfo.Role == "ski_patrol"
    }

    actionsAvailable() {
        return this.updateActionAvailable() || this.deleteActionAvailable() || this.markCheckedActionAvailable()
    }

    public render() {
        if (this.props.userInfo == undefined) {
            //return <Redirect to='/notfound' />
            return null
        }

        if (this.props.isLoading == false && this.props.messagesList.length == 0) {
            this.props.getMessages();
        }

        let messagesTable: any;
        if (this.props.isLoading == true) { 
            messagesTable = (<LoadingScreen />);
        }
        else {
            let filteredMessagesList: MessagesStore.Message[]
            if (this.state.searchMessageID != null) {
                filteredMessagesList = this.props.messagesList.filter(
                    message => (this.state.searchMessageID == null || message.MessageID.toString().includes(this.state.searchMessageID.toString()))
                )
            }
            else {
                filteredMessagesList = this.props.messagesList
            }
            messagesTable = this.renderMessagesTable(filteredMessagesList);
        }
        
        return (
            <div className={classes.main_div} >

                {this.state.updateModalWindow != null && this.state.updateMessageModalVisible && this.state.updateModalWindow}
                <Styles.HeaderText>
                    Messages
                </Styles.HeaderText>

                {this.sendActionAvailable() &&
                    <div className={classesModal.modalWindowDiv} style={{padding: "10px"} }>
                        <InputCell
                            whatToInput="Your message:"
                            value={this.state.currentText}
                            onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                                this.setState({
                                    ...this.state, currentText: event.target.value
                                })
                                event.preventDefault();
                            }}
                            type="text"
                        />
                        <UsualButton onClick={(event: React.FormEvent<HTMLFormElement>) => {
                            this.props.sendMessage(this.state.currentText)
                            this.setState({ ...this.state, currentText: "" })
                            if (this.messagesAvailable())
                                this.props.getMessages();
                        }}>
                            Send
                        </UsualButton>
                    </div>
                }

                {this.searchActionAvailable() && 
                    <div>
                        <InputCell
                            whatToInput="Find by ID:"
                            value={this.state.searchMessageID != null ? this.state.searchMessageID : ''}
                            onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                                this.setState({ searchMessageID: event.target.value as any as number })
                            }}
                            type="number"
                        />
                        {messagesTable}
                    </div>
                }

            </div>
        );
    }

    private renderMessagesTable(messages: MessagesStore.Message[]) {
        return <MyTable
            headNames={['messageID', 'senderID', 'checkedByID', 'text', this.actionsAvailable() ? 'actions' : '']}
            rows={messages.map((message: MessagesStore.Message) =>
                [message.MessageID,
                    message.SenderID,
                    message.CheckedByID == 0 ? 'nobody' : message.CheckedByID,
                    message.Text,

                    <div style={{ display: 'flex', margin: "0 -5px" }}>

                        <div style={{ flex: "1 1 auto", margin: "0 5px" }}>
                            {this.markCheckedActionAvailable() && message.CheckedByID == 0 &&
                                <MarkButton onClick={(event: React.FormEvent<HTMLFormElement>) => {

                                this.processMarkAction(message.MessageID)
                                console.log('here')
                            }
                            } />
                            }
                        </div>

                        <div style={{ flex: "1 1 auto", margin: "0 5px" }}>
                            {this.deleteActionAvailable() &&
                                <DeleteButton onClick={(event: React.FormEvent<HTMLFormElement>) => this.props.deleteMessageByID(message.MessageID)} />
                            }
                        </div>

                        <div style={{ flex: "1 1 auto", margin: "0 5px" }}>
                            {this.updateActionAvailable() &&
                                <UpdateButton style={{ flex: "1 1 auto", margin: "0 5px" }} onClick={(event: React.FormEvent<HTMLFormElement>) => {
                                    this.setState({
                                        ...this.state,
                                        updateMessageModalVisible: true,
                                        updateModalWindow:
                                            <ModalWindow visible={true} setVisible={(value: boolean) => this.setState({
                                                ...this.state,
                                                updateMessageModalVisible: value
                                            })}>
                                                <MessageForm formTitle="Update Message" messageID={message.MessageID} senderID={message.SenderID}
                                                    checkedByID={message.CheckedByID} text={message.Text}
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
    ...state.messages
});

const mapDispatchToProps = ({
    ...UserStore.actionCreators,
    ...MessagesStore.actionCreators
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Messages as any);
