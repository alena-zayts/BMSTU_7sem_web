import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LiftsListState {
    isLoading: boolean;
    liftsList: Lift[];
}

export interface Lift {
    LiftID: number;
    LiftName: string;
    IsOpen: boolean;
    SeatsAmount: number;
    LiftingTime: number;
    QueueTime: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestLiftsListAction {
    type: 'REQUEST_LIFTS_LIST';
}

interface ReceiveLiftsListAction {
    type: 'RECEIVE_LIFTS_LIST';
    liftsList: Lift[];
}

interface DeleteLiftByNameAction {
    type: 'DELETE_LIFT_BY_NAME_ACTION';
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestLiftsListAction | ReceiveLiftsListAction | DeleteLiftByNameAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestLiftsList: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.liftsList) {
            fetch(`api/lifts`)
                .then(response =>
                    response.json() as Promise<Lift[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_LIFTS_LIST', liftsList: data });
                });

            dispatch({ type: 'REQUEST_LIFTS_LIST', }); //надо закоментить?
        }
    },

    deleteLiftByName: (liftName: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        fetch('lifts/' + liftName, { method: 'DELETE', })
            .then(res => res.text()) // or res.json()
            .then(res => console.log(res))

        dispatch({ type: 'REQUEST_LIFTS_LIST', });
    }
};



// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LiftsListState = { liftsList: [], isLoading: false };

export const reducer: Reducer<LiftsListState> = (state: LiftsListState | undefined, incomingAction: Action): LiftsListState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_LIFTS_LIST':
            return {
                liftsList: state.liftsList,
                isLoading: true
            };
        case 'RECEIVE_LIFTS_LIST':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            //if (action.startDateIndex === state.startDateIndex) {
                return {
                    liftsList: action.liftsList,
                    isLoading: false
                };
            //}
            break;
    }

    return state;
};
