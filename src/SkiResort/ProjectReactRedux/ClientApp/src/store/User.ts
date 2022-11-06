import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import authHeader from '../helpers/auth-header';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    userInfo?: UserInfo,
    userToken?: string,
    error?: string,
}

export interface UserInfo {
    UserID: number;
    UserEmail: string;
    Password: string;
    Role: string;
    CardID: number;
}

interface TokenRespones {
    access_token: string,
    username: string,
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface UserLogInSuccessAction {
    type: 'USER_LOG_IN_SUCCESS';
    userToken: string
}

interface UserLogInFailedAction {
    type: 'USER_LOG_IN_FAILED';
    error: string
}

interface UserRegisterSuccessAction {
    type: 'USER_REGISTER_SUCCESS';
    userToken: string
}

interface UserRegisterFailedAction {
    type: 'USER_REGISTER_FAILED';
    error: string
}

interface UserLogOutAction {
    type: 'USER_LOG_OUT';
}

interface GetUserInfoSuccessAction {
    type: 'GET_USER_INFO_SUCCESS';
    userInfo: UserInfo;
}

interface GetUserInfoFailedAction {
    type: 'GET_USER_INFO_FAILED';
    error: string
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = UserLogInSuccessAction | UserLogInFailedAction | GetUserInfoSuccessAction | GetUserInfoFailedAction | UserLogOutAction
    | UserRegisterFailedAction | UserRegisterSuccessAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    logIn: (userEmail: string, userPassword: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        fetch('api/account/login' + '?userEmail=' + userEmail + '&userPassword=' + userPassword, { method: 'POST', })
            .then(response => {
                if (!response.ok) { throw response }
                return response.json() as Promise<TokenRespones>
            })
            .then(data => {
                localStorage.setItem("userToken", JSON.stringify(data.access_token));
                dispatch({ type: 'USER_LOG_IN_SUCCESS', userToken: data.access_token });
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Incorrect email or password")
                    console.log('error from func', errorMessage)
                    dispatch({ type: 'USER_LOG_IN_FAILED', error: errorMessage })
                })
            })

    },
    getUserInfo: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.user.userInfo == undefined) {
            fetch('api/account', { method: 'GET', headers: authHeader() })
                .then(response => {
                    if (!response.ok) { throw response }
                    return response.json() as Promise<UserInfo>
                })
                .then(data => {
                    dispatch({ type: 'GET_USER_INFO_SUCCESS', userInfo: data });
                })
                .catch(err => {
                    err.text().then((errorMessage: string) => {
                        alert("Couldn't get userInfo")
                        console.log('error from func', errorMessage)
                        dispatch({ type: 'GET_USER_INFO_FAILED', error: errorMessage })
                    })
                })

            //fetch('api/account', { method: 'GET', headers: authHeader() })
            //    .then(response => response.json() as Promise<UserInfo>)
            //    .then(data => {
            //        dispatch({ type: 'RECIEVE_USER_INFO', userInfo: data });
            //    });
        }
    },
    logOut: (): AppThunkAction<KnownAction> => (dispatch) => {
        if (localStorage.getItem('userToken')) {
            localStorage.removeItem('userToken')
        }
        dispatch({ type: 'USER_LOG_OUT' })
    },

    register: (userEmail: string, userPassword: string, cardID: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        fetch(`api/account/register?userEmail=${userEmail}&userPassword=${userPassword}&cardID=${cardID}`, { method: 'POST', })
            .then(response => {
                if (!response.ok) { throw response }
                return response.json() as Promise<TokenRespones>
            })
            .then(data => {
                localStorage.setItem("userToken", JSON.stringify(data.access_token));
                dispatch({ type: 'USER_REGISTER_SUCCESS', userToken: data.access_token });
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Repeated email i guess")
                    console.log('error from func', errorMessage)
                    dispatch({ type: 'USER_REGISTER_FAILED', error: errorMessage })
                })
            })

    },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const parseJwt = (token: string) => {
    try {
        return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
        return null;
    }
};


const userTokenUnparsed: string | null = localStorage.getItem('userToken');
let userTokenFromStorage: string | undefined = undefined;
if (userTokenUnparsed != undefined && userTokenUnparsed != "undefined") {
    userTokenFromStorage = JSON.parse(userTokenUnparsed);
    if (userTokenFromStorage) {
        const decodedJwt = parseJwt(userTokenFromStorage);
        if (decodedJwt.exp * 1000 < Date.now()) {
            userTokenFromStorage = undefined;
            localStorage.removeItem('userToken')
        }
    }
}
console.log('userTokenFromStorage from defaultState')
console.log(userTokenFromStorage)

const defaultState: UserState = {
    userInfo: undefined,
    userToken: userTokenFromStorage,
    error: undefined,
};

export const reducer: Reducer<UserState> = (state: UserState | undefined, incomingAction: Action): UserState => {
    if (state === undefined) {
        return defaultState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'USER_LOG_IN_SUCCESS':
            return {
                userInfo: undefined,
                userToken: action.userToken,
                error: undefined
            };
        case 'USER_LOG_IN_FAILED':
            return {
                userInfo: undefined,
                userToken: undefined,
                error: action.error
            };
        case 'USER_LOG_OUT':
            return {
                userToken: undefined,
                error: undefined,
                userInfo: undefined
            };
        case 'GET_USER_INFO_SUCCESS':
            return {
                error: undefined,
                userToken: state.userToken,
                userInfo: action.userInfo
            };
        case 'GET_USER_INFO_FAILED':
            return {
                userToken: state.userToken,
                error: action.error,
                userInfo: undefined
            };

        case 'USER_REGISTER_SUCCESS':
            return {
                userInfo: undefined,
                userToken: action.userToken,
                error: undefined
            };
        case 'USER_REGISTER_FAILED':
            return {
                userInfo: undefined,
                userToken: undefined,
                error: action.error
            };

        break;
    }

    return state;
};
