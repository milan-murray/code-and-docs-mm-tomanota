//
// Base functions used to get data from the API 
//


// API Base URL - the server address
const BASE_URL = 'http://localhost:5000/quiz';

// Get HTTP headers for requests to the api
function getHeaders() {
    // Return headers
    // Note that the access token is set in the Authorization: Bearer
    return new Headers({
        "Accept": "application/json",
        "Content-Type": "application/json",
        //"Authorization": "Bearer " + getAccessToken()
    });
}

//
// Used to Initialise  requests and permit cross origin requests
// parameters: http method and body content note default values)
function fetchInit(httpMethod = 'GET', bodyContent = '') {

    let init = {
        method: httpMethod,
        //credentials: 'include',
        headers: getHeaders(),
        mode: 'cors',
        cache: 'default',
    };

    if (bodyContent != '') {
        init.body = bodyContent
    }

    return init;
};

//
// Asynchronous Function getDataAsync from a url and return
async function getDataAsync(url, init = fetchInit()) {
    try {
        // Call fetch and await the respose
        // Initally returns a promise
        const response = await fetch(url, init);
        // As Resonse is dependant on the fetch call, await must also be used here
        const json = await response.json();
        // Output result to console (for testing purposes) 
        console.log(json);

        // return data
        return json;

        // catch and log any errors
    } catch (err) {
        console.log(err);
        return err;
    }
} // End function


// export this as a module
export {
    BASE_URL,
    fetchInit,
    getDataAsync
};