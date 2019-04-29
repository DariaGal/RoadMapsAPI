exports.registerNewUser = async(client, login, password) => {
    const response = await client.post('auth/signup', {
        Login: login,
        Password: password 
    });

    return response.data.accessToken;
}

exports.authenticate = async(client, login, password) => {
    const response = await client.post('auth/signin', {
        Login: login,
        Password: password 
    });

    return response.data.accessToken;
}