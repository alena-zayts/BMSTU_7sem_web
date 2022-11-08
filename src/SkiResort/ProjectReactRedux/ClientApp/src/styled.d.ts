import 'styled-components';

import { ITheme } from './styles/itheme';

declare module 'styled-components' {
    export interface DefaultTheme extends ITheme { }
}