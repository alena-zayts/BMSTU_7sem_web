import { RouteComponentProps } from 'react-router';
import * as LiftsStore from '../../store/Lifts';
import * as UserStore from '../../store/User'

// At runtime, Redux will merge together...
export type LiftsProps =
    //{ user: UserStore.UserInfo, lifts: LiftsStore.LiftsState, } // ... state we've requested from the Redux store
    UserStore.UserInfo
    & LiftsStore.LiftsState
    & typeof LiftsStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{}>; // ... plus incoming routing parameters