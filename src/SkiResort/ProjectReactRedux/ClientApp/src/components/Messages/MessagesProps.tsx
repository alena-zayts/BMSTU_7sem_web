import { RouteComponentProps } from 'react-router';
import * as UserStore from '../../store/User';
import * as MessagesStore from '../../store/Messages';

// At runtime, Redux will merge together...
export type MessagesProps =
    UserStore.UserState
    & UserStore.UserInfo
    & MessagesStore.MessagesState
    & typeof MessagesStore.actionCreators // ... plus action creators we've requested
    & typeof UserStore.actionCreators
    & RouteComponentProps<{}>; // ... plus incoming routing parameters