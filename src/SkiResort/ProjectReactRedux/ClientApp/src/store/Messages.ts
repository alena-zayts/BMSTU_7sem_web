import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import authHeader from '../helpers/auth-header';

export interface MessagesState {
    messagesList: Message[];
    error?: string,
    isLoading: boolean
}

export interface Message {
    MessageID: number;
    SenderID: number;
    CheckedByID: number;
    Text: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface GetMessagesRequested {
    type: 'GET_MESSAGES_REQUESTED';
}

interface GetMessagesSuccessed {
    type: 'GET_MESSAGES_SUCCESSED';
    messagesList: Message[];
}

interface GetMessagesFailed {
    type: 'GET_MESSAGES_FAILED';
    error: string;
}

interface DeleteMessageByIDRequested {
    type: 'DELETE_MESSAGE_BY_ID_REQUESTED';
}

interface DeleteMessageByIDFailed {
    type: 'DELETE_MESSAGE_BY_ID_FAILED';
    error: string;
}

interface DeleteMessageByIDSuccessed {
    type: 'DELETE_MESSAGE_BY_ID_SUCCESSED';
}

interface SendMessageRequested {
    type: 'SEND_MESSAGE_REQUESTED';
}

interface SendMessageSuccessed {
    type: 'SEND_MESSAGE_SUCCESSED';
}

interface SendMessageFailed {
    type: 'SEND_MESSAGE_FAILED';
    error: string;
}

interface MarkMessageReadRequested {
    type: 'MARK_MESSAGE_READ_REQUESTED';
}

interface MarkMessageReadSuccessed {
    type: 'MARK_MESSAGE_READ_SUCCESSED';
}

interface MarkMessageReadFailed {
    type: 'MARK_MESSAGE_READ_FAILED';
    error: string;
}

interface UpdateMessageRequested {
    type: 'UPDATE_MESSAGE_REQUESTED';
}

interface UpdateMessageSuccessed {
    type: 'UPDATE_MESSAGE_SUCCESSED';
}

interface UpdateMessageFailed {
    type: 'UPDATE_MESSAGE_FAILED';
    error: string;
}



// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = GetMessagesRequested | GetMessagesSuccessed | GetMessagesFailed |
    DeleteMessageByIDRequested | DeleteMessageByIDFailed | DeleteMessageByIDSuccessed |
    SendMessageRequested | SendMessageFailed | SendMessageSuccessed |
    MarkMessageReadFailed | MarkMessageReadRequested | MarkMessageReadSuccessed |
    UpdateMessageRequested | UpdateMessageFailed | UpdateMessageSuccessed;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    getMessages: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.messages) {
            dispatch({ type: 'GET_MESSAGES_REQUESTED' });
            fetch('api/messages', { method: 'GET', headers: authHeader()})
                .then(response => {
                    if (!response.ok) { throw response }
                    return response.json() as Promise<Message[]>
                })
                .then(data => {
                    dispatch({ type: 'GET_MESSAGES_SUCCESSED', messagesList: data });
                })
                .catch(err => {
                    console.log(err)
                    err.text().then((errorMessage: string) => {
                        alert("Error while getting messagesList\n" + errorMessage)
                        dispatch({ type: 'GET_MESSAGES_FAILED', error: errorMessage })
                    })
                })
        }
        
    },

    deleteMessageByID: (messageID: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'DELETE_MESSAGE_BY_ID_REQUESTED' });
        fetch(`api/messages/${messageID}`, { method: 'DELETE', headers: authHeader() })
            .then(response => {
                if (!response.ok) { throw response }
                dispatch({ type: 'DELETE_MESSAGE_BY_ID_SUCCESSED' });
                dispatch(actionCreators.getMessages())
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Error while deleteMessageByID\n" + errorMessage)
                    dispatch({ type: 'DELETE_MESSAGE_BY_ID_FAILED', error: errorMessage })
                })
            })
    },

    sendMessage: (text: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'SEND_MESSAGE_REQUESTED' });

        let query: string = `api/messages?text=${text}`
        fetch(query, { method: 'POST', headers: authHeader() })
            .then(response => {
                if (!response.ok) { throw response }
                dispatch({ type: 'SEND_MESSAGE_SUCCESSED' });
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Error while sendMessage\n" + errorMessage)
                    dispatch({ type: 'SEND_MESSAGE_FAILED', error: errorMessage })
                })
            })
    },

    markMessageRead: (messageID: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'MARK_MESSAGE_READ_REQUESTED' });

        let query: string = `api/messages?messageID=${messageID}`
        fetch(query, { method: 'PATCH', headers: authHeader() })
            .then(response => {
                if (!response.ok) { throw response }
                dispatch({ type: 'MARK_MESSAGE_READ_SUCCESSED' });
                dispatch(actionCreators.getMessages())
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Error while markMessageRead\n" + errorMessage)
                    dispatch({ type: 'MARK_MESSAGE_READ_FAILED', error: errorMessage })
                })
            })
    },


    updateMessage: (messageID: number, senderID: number, checkedByID: number, text:string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'UPDATE_MESSAGE_REQUESTED' });
        fetch(`api/messages/${messageID}?senderID=${senderID}&checkedByID=${checkedByID}&text=${text}`, { method: 'PATCH', headers: authHeader() })
            .then(response => {
                if (!response.ok) { throw response }
                dispatch({ type: 'UPDATE_MESSAGE_SUCCESSED' });
                dispatch(actionCreators.getMessages())
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Error while updateMessage\n" + errorMessage)
                    dispatch({ type: 'UPDATE_MESSAGE_FAILED', error: errorMessage })
                })
            })
    },

};



// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: MessagesState = { messagesList: [], isLoading: false, error: undefined };

export const reducer: Reducer<MessagesState> = (state: MessagesState | undefined, incomingAction: Action): MessagesState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        
        case 'GET_MESSAGES_REQUESTED':
            return {
                messagesList: state.messagesList,
                isLoading: true
            };
        case 'GET_MESSAGES_SUCCESSED':
            return {
                messagesList: action.messagesList,
                isLoading: false
            };
        case 'GET_MESSAGES_FAILED':
            return {
                messagesList: state.messagesList,
                isLoading: false,
                error: action.error
            };
        case 'DELETE_MESSAGE_BY_ID_REQUESTED':
            return {
                messagesList: state.messagesList,
                isLoading: true
            };
        case 'DELETE_MESSAGE_BY_ID_FAILED':
            return {
                messagesList: state.messagesList,
                isLoading: false,
                error: action.error
            };
        case 'DELETE_MESSAGE_BY_ID_SUCCESSED':
            return {
                messagesList: state.messagesList,
                isLoading: false,
            };
        case 'UPDATE_MESSAGE_REQUESTED':
            return {
                messagesList: state.messagesList,
                isLoading: true
            };
        case 'UPDATE_MESSAGE_FAILED':
            return {
                messagesList: state.messagesList,
                isLoading: false,
                error: action.error
            };
        case 'UPDATE_MESSAGE_SUCCESSED':
            return {
                messagesList: state.messagesList,
                isLoading: false,
            };
        break;
    }

    return state;
};
