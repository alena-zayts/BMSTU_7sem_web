import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    loading: boolean,
    userInfo?: UserInfo,
    userToken?: string,
    error?: string,
    success: boolean,
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

interface UserLogInAction {
    type: 'USER_LOG_IN';
}

interface RecieveTokenAction {
    type: 'RECEIVE_TOKEN';
    userToken: string
}

interface RecieveUserInfoAction {
    type: 'RECIEVE_USER_INFO_ACTION';
    userInfo: UserInfo;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = UserLogInAction | RecieveTokenAction | RecieveUserInfoAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    logIn: (userEmail: string, userPassword: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        fetch('api/account/login' + '?userEmail=' + userEmail + '&userPassword=' + userPassword, { method: 'POST', })
            .then(response => response.json() as Promise<TokenRespones>)
            .then(data => {
                console.log('from action logIn')
                console.log(data.access_token)
                sessionStorage.setItem('jwtToken', data.access_token); ////
                dispatch({ type: 'RECEIVE_TOKEN', userToken: data.access_token});
            });

        //dispatch(signUpUser(values))
        //    .then((response) => {
        //        let data = response.payload.data;
        //        //if any one of these exist, then there is a field error 
        //        if (response.payload.status != 200) {
        //            //let other components know
        //            dispatch(signUpUserFailure(response.payload));
        //            reject(data); //this is for redux-form itself
        //        } else {
        //            //store JWT Token to browser session storage 
        //            //If you use localStorage instead of sessionStorage, then this w/   
        //            //persisted across tabs and new windows.
        //            //sessionStorage = persisted only in current tab

        //            sessionStorage.setItem(‘jwtToken’, response.payload.data.token);

        //            //let other components know that we got user and things are fine
        //            dispatch(signUpUserSuccess(response.payload));
        //            resolve();//this is for redux-form itself
        //        }
        //    });

    },
    getUserInfo: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        fetch('api/account/', { method: 'GET', })
            .then(response => response.json() as Promise<UserInfo>)
            .then(data => {
                console.log('from action getUserInfo')
                console.log(data)
                dispatch({ type: 'RECIEVE_USER_INFO_ACTION', userInfo: data });
            });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const defaultState: UserState = {
    loading: false,
    userInfo: undefined,
    userToken: undefined,
    error: undefined,
    success: false,
};

export const reducer: Reducer<UserState> = (state: UserState | undefined, incomingAction: Action): UserState => {
    if (state === undefined) {
        return defaultState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'USER_LOG_IN':
            console.log('from USER_LOG_IN')
            return {
                loading: true,
                success: false
            };
        case 'RECEIVE_TOKEN':
            console.log('from RECEIVE_TOKEN')
            console.log(action.userToken)
            return {
                loading: false,
                success: true,
                userToken: action.userToken
            };
        case 'RECIEVE_USER_INFO_ACTION':
            return {
                loading: false,
                success: true,
                userInfo: action.userInfo
            };
        break;
    }

    return state;
};
