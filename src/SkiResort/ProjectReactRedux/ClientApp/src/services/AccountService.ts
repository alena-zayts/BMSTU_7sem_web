// services are state-less
// they act as utility facades that abstract the details for complex operations
// normally, our interface to any sort of server API will be as a service



interface GetTokenResponse {
    token: string,
    username: string, //unused
}

interface AccountInfo {
    UserEmail: string;
    Password: string,
    Role: string,
    CardID: number,
    UserID: number
}

interface LogInResponse {
    token: string,
    accountInfo: AccountInfo

}

class AccountService {
    myHeaders = { Accept: 'application/json', Authorization: null }

    async logIn(userEmail: string, userPassword: string): Promise<LogInResponse> {
        const url = `api/account/login?userEmail=${userEmail}&userPassword=${userPassword}`;
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                Accept: 'application/json'
            }
        });
        if (!response.ok) {
            throw new Error(`AccountService logIn failed, HTTP status ${response.status}`);
        }
        const data: Promise<GetTokenResponse> = await response.json();
        const token: string = (await data).token;
        const accountInfo: Promise<AccountInfo> = this.getAccountInfoByToken(token) 
        return {
            token: token,
            accountInfo: accountInfo
        }
    }

    async getAccountInfoByToken(token: string): Promise<AccountInfo> {
        const url = `api/account/`;
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                Accept: 'application/json',
                Authorization: token
            }
        });
        if (!response.ok) {
            throw new Error(`AccountService getAccountInfoByToken failed, HTTP status ${response.status}`);
        }
        const data = await response.json() as Promise<AccountInfo>;
        return data;
    }

}

//logIn: (userEmail: string, userPassword: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
//    fetch('api/account/login' + '?userEmail=' + userEmail + '&userPassword=' + userPassword, { method: 'POST', })
//        .then(response => response.json() as Promise<TokenRespones>)
//        .then(data => {
//            console.log('from action logIn')
//            console.log(data.access_token)
//            dispatch({ type: 'RECEIVE_TOKEN', userToken: data.access_token });
//        });

export default new AccountService();