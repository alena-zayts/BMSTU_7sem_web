import { createXHRAction } from './Helper';

export function getUserInfoByTocken() {
    return (dispatch) => {
        return createXHRAction('account', dispatch, {
            method: 'get',
            url: `/api/account`
        });
    };
}