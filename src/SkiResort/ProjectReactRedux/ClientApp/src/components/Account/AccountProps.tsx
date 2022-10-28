import { RouteComponentProps } from 'react-router';
import * as UserStore from '../../store/User';

// At runtime, Redux will merge together...
export type AccountProps =
    UserStore.UserState // ... state we've requested from the Redux store
    & typeof UserStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{}>; // ... plus incoming routing parameters