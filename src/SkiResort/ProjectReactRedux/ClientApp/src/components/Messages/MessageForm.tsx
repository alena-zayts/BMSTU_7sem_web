import React, { ReactNode } from 'react';
import * as Styles from '../../styles/components'
import { connect } from 'react-redux';
import UsualButton from '../UI/usualButton/UsualButton';
import InputCell from '../UI/inputCell/InputCell';
import { ApplicationState } from '../../store';
import * as MessagesStore from '../../store/Messages';
import classes from '../UI/modalWindow/ModalWindow.module.css';

interface MessageFormProps {
    messageID: number,
    senderID: number,
    checkedByID: number,
    text: string,
    formTitle: string,
    buttonText: string,
    buttonAction: (
        messageID: number,
        senderID: number,
        checkedByID: number,
        text: string,
    ) => void
}

class MessageForm extends React.PureComponent<MessageFormProps,
    {
        messageID: number,
        senderID: number,
        checkedByID: number,
        text: string, }>
{
    constructor(props: MessageFormProps) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.state = {messageID: props.messageID, senderID: props.senderID, checkedByID: props.checkedByID,text: props.text,
        };

    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        this.props.buttonAction(this.state.messageID, this.state.senderID, this.state.checkedByID, this.state.text);
    }

    render() {
        return (
            <div className={classes.modalWindowDiv} >
                <Styles.HeaderText> {this.props.formTitle} </Styles.HeaderText>
                <InputCell
                    whatToInput="messageID:"
                    value={this.state.messageID}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, messageID: e.target.value as any as number})}
                    type="number"
                />
                <InputCell
                    whatToInput="senderID:"
                    value={this.state.senderID}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, senderID: e.target.value as any as number })}
                    type="number"
                />
                <InputCell
                    whatToInput="checkedByID:"
                    value={this.state.checkedByID}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, checkedByID: e.target.value as any as number })}
                    type="number"
                />
               
                <InputCell
                    whatToInput="text:"
                    value={this.state.text}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => this.setState({ ...this.state, text: e.target.value })}
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
    (state: ApplicationState) => state.messages,
    MessagesStore.actionCreators
)(MessageForm as any);