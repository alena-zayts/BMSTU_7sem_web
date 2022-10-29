// reducers hold the store's state (the initialState object defines it)
// reducers also handle plain object actions and modify their state (immutably) accordingly
// this is the only way to change the store's state
// the other exports in this file are selectors, which is business logic that digests parts of the store's state
// for easier consumption by views

// для обеспечения иммутабельности состояния (как того требует Redux)
import Immutable from 'seamless-immutable';

export interface AccountState {
    AccountInfo?: AccountInfo,
    Token?: string,
}

export interface AccountInfo {
    UserID: number;
    UserEmail: string;
    Password: string;
    Role: string;
    CardID: number;
}

const initialState: AccountState = Immutable({
    AccountInfo: undefined,
    Token: undefined,
});

export default function reduce(state = initialState, action = {}) {
    switch (action.type) {
        default:
            return state;
    }
}