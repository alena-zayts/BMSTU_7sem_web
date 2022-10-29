export function createXHRAction(xhrType, dispatch, options) {
    // you can customize createXHRAction here with options parameter.

    dispatch({ type: xhrType, data: { fetching: true, data: [] } });

    return new Promise((resolve, reject) => {
        fetch(options.url, { method: options.method, })
            .then((response) => {
                // to getting server response, you must use .json() method and this is promise object
                let parseJSONPromise = response.json();

                if (response.status >= 200 && response.status < 300) {
                    parseJSONPromise.then((result) => {
                        dispatch({ type: xhrType, data: { fetching: false, data: result.data } });
                        resolve(result.data);
                    });
                    return parseJSONPromise;    // make possible to use then where calling this
                }
                else {
                    return parseJSONPromise.then(res => {
                        reject({ message: res.error.message });
                    });
                }
            })
            .catch((error) => {
                console.log('error in createXHRAction: ')
                console.log(error)
            });
    });
}