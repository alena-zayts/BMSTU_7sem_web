import { RouteComponentProps } from 'react-router';
import * as UserStore from '../../store/User';
import * as LiftsStore from '../../store/Lifts';

// At runtime, Redux will merge together...
export type LiftsProps =
    UserStore.UserState
    & UserStore.UserInfo
    & LiftsStore.LiftsState
    & typeof LiftsStore.actionCreators // ... plus action creators we've requested
    & typeof UserStore.actionCreators
    & RouteComponentProps<{}>; // ... plus incoming routing parameters