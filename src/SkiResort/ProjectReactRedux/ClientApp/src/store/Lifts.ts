import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LiftsState {
    isLoading: boolean;
    lifts: Lift[];
}

export interface Lift {
    liftID: number;
    liftName: string;
    isOpen: boolean;
    seatsAmount: number;
    liftingTime: number;
    queueTime: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestLiftsAction {
    type: 'REQUEST_LIFTS';
}

interface ReceiveLiftsAction {
    type: 'RECEIVE_LIFTS';
    lifts: Lift[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestLiftsAction | ReceiveLiftsAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestLifts: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.lifts) {
            fetch(`lifts`)
                .then(response => response.json() as Promise<Lift[]>)
                .then(data => {
                    console.log(data),
                    dispatch({ type: 'RECEIVE_LIFTS', lifts: data });
                });

            dispatch({ type: 'REQUEST_LIFTS', });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LiftsState = { lifts: [], isLoading: false };

export const reducer: Reducer<LiftsState> = (state: LiftsState | undefined, incomingAction: Action): LiftsState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_LIFTS':
            return {
                lifts: state.lifts,
                isLoading: true
            };
        case 'RECEIVE_LIFTS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            //if (action.startDateIndex === state.startDateIndex) {
                return {
                    lifts: action.lifts,
                    isLoading: false
                };
            //}
            break;
    }

    return state;
};
