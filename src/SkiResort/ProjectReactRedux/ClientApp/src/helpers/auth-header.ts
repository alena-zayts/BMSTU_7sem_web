export default function authHeader(): HeadersInit {
    const requestHeaders: HeadersInit = new Headers();
    
    const userTokenFromStorage: string | null = localStorage.getItem('userToken');
    
    if (userTokenFromStorage) {
        const userTokenString: string = JSON.parse(userTokenFromStorage);
        requestHeaders.set('Authorization', 'Bearer ' + userTokenString);
    }
    return requestHeaders;
}