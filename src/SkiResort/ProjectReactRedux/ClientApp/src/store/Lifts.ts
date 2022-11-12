import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import authHeader from '../helpers/auth-header';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LiftsState {
    liftsList: Lift[];
    error?: string,
    isLoading: boolean
}

export interface Lift {
    LiftID: number;
    LiftName: string;
    IsOpen: boolean;
    SeatsAmount: number;
    LiftingTime: number;
    QueueTime: number;
    ConnectedSlopeNames: string[] | null;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface GetLiftsRequested {
    type: 'GET_LIFTS_REQUESTED';
}

interface GetLiftsSuccessed {
    type: 'GET_LIFTS_SUCCESSED';
    liftsList: Lift[];
}

interface GetLiftsFailed {
    type: 'GET_LIFTS_FAILED';
    error: string;
}

interface DeleteLiftByNameRequested {
    type: 'DELETE_LIFT_BY_NAME_REQUESTED';
}

interface DeleteLiftByNameFailed {
    type: 'DELETE_LIFT_BY_NAME_FAILED';
    error: string;
}

interface DeleteLiftByNameSuccessed {
    type: 'DELETE_LIFT_BY_NAME_SUCCESSED';
}

interface AddLiftRequested {
    type: 'ADD_LIFT_REQUESTED';
}

interface AddLiftSuccessed {
    type: 'ADD_LIFT_SUCCESSED';
}

interface AddLiftFailed {
    type: 'ADD_LIFT_FAILED';
    error: string;
}

interface UpdateLiftRequested {
    type: 'UPDATE_LIFT_REQUESTED';
}

interface UpdateLiftSuccessed {
    type: 'UPDATE_LIFT_SUCCESSED';
}

interface UpdateLiftFailed {
    type: 'UPDATE_LIFT_FAILED';
    error: string;
}



// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = GetLiftsRequested | GetLiftsSuccessed | GetLiftsFailed |
    DeleteLiftByNameRequested | DeleteLiftByNameFailed | DeleteLiftByNameSuccessed |
    AddLiftRequested | AddLiftSuccessed | AddLiftFailed |
    UpdateLiftRequested | UpdateLiftFailed | UpdateLiftSuccessed;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    getLifts: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.lifts) {// && appState.lifts.isLoading == false) {
            dispatch({ type: 'GET_LIFTS_REQUESTED' });
            fetch('api/lifts', { method: 'GET', headers: authHeader()})
                .then(response => {
                    if (!response.ok) { throw response }
                    return response.json() as Promise<Lift[]>
                })
                .then(data => {
                    dispatch({ type: 'GET_LIFTS_SUCCESSED', liftsList: data });
                })
                .catch(err => {
                    console.log(err)
                    err.text().then((errorMessage: string) => {
                        alert("Error while getting liftsList\n" + errorMessage)
                        dispatch({ type: 'GET_LIFTS_FAILED', error: errorMessage })
                    })
                })
        }
        
    },

    deleteLiftByName: (liftName: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'DELETE_LIFT_BY_NAME_REQUESTED' });
        fetch(`api/lifts/${liftName}`, { method: 'DELETE', headers: authHeader() })
            .then(response => {
                if (!response.ok) { throw response }
                dispatch({ type: 'DELETE_LIFT_BY_NAME_SUCCESSED' });
                dispatch(actionCreators.getLifts())
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Error while deleteLiftByName\n" + errorMessage)
                    dispatch({ type: 'DELETE_LIFT_BY_NAME_FAILED', error: errorMessage })
                })
            })
    },

    addLift: (liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number, connectedSlopeNames: string[]): AppThunkAction<KnownAction> => (dispatch, getState) => {
        console.log('slopeNames from addLift', connectedSlopeNames)
        
        dispatch({ type: 'ADD_LIFT_REQUESTED' });

        let query: string = `api/lifts?liftName=${liftName}&isOpen=${isOpen}&seatsAmount=${seatsAmount}&liftingTime=${liftingTime}`
        connectedSlopeNames.forEach(slopeName => query += `&connectedSlopeNames=${slopeName}`)

        fetch(query, { method: 'POST', headers: authHeader() })
            .then(response => {
                if (!response.ok) { throw response }
                dispatch({ type: 'ADD_LIFT_SUCCESSED' });
                dispatch(actionCreators.getLifts())
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Error while addLift\n" + errorMessage)
                    dispatch({ type: 'ADD_LIFT_FAILED', error: errorMessage })
                })
            })
    },

    updateLift: (liftName: string, isOpen: boolean, seatsAmount: number, liftingTime: number, connectedSlopeNames: string[]): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'UPDATE_LIFT_REQUESTED' });
        fetch(`api/lifts/${liftName}?isOpen=${isOpen}&seatsAmount=${seatsAmount}&liftingTime=${liftingTime}` + connectedSlopeNames.map((slopeName: string) => `&connectedSlopeNames=${slopeName}`), { method: 'PATCH', headers: authHeader() })
            .then(response => {
                if (!response.ok) { throw response }
                dispatch({ type: 'UPDATE_LIFT_SUCCESSED' });
                dispatch(actionCreators.getLifts())
            })
            .catch(err => {
                err.text().then((errorMessage: string) => {
                    alert("Error while updateLift\n" + errorMessage)
                    dispatch({ type: 'UPDATE_LIFT_FAILED', error: errorMessage })
                })
            })
    },

};



// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LiftsState = { liftsList: [], isLoading: false, error: undefined };

export const reducer: Reducer<LiftsState> = (state: LiftsState | undefined, incomingAction: Action): LiftsState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        
        case 'GET_LIFTS_REQUESTED':
            return {
                liftsList: state.liftsList,
                isLoading: true
            };
        case 'GET_LIFTS_SUCCESSED':
            return {
                liftsList: action.liftsList,
                isLoading: false
            };
        case 'GET_LIFTS_FAILED':
            return {
                liftsList: state.liftsList,
                isLoading: false,
                error: action.error
            };
        case 'DELETE_LIFT_BY_NAME_REQUESTED':
            return {
                liftsList: state.liftsList,
                isLoading: true
            };
        case 'DELETE_LIFT_BY_NAME_FAILED':
            return {
                liftsList: state.liftsList,
                isLoading: false,
                error: action.error
            };
        case 'DELETE_LIFT_BY_NAME_SUCCESSED':
            return {
                liftsList: state.liftsList,
                isLoading: false,
            };
        case 'ADD_LIFT_REQUESTED':
            return {
                liftsList: state.liftsList,
                isLoading: true
            };
        case 'ADD_LIFT_FAILED':
            return {
                liftsList: state.liftsList,
                isLoading: false,
                error: action.error
            };
        case 'ADD_LIFT_SUCCESSED':
            return {
                liftsList: state.liftsList,
                isLoading: false,
            };
        case 'UPDATE_LIFT_REQUESTED':
            return {
                liftsList: state.liftsList,
                isLoading: true
            };
        case 'UPDATE_LIFT_FAILED':
            return {
                liftsList: state.liftsList,
                isLoading: false,
                error: action.error
            };
        case 'UPDATE_LIFT_SUCCESSED':
            return {
                liftsList: state.liftsList,
                isLoading: false,
            };
        break;
    }

    return state;
};
