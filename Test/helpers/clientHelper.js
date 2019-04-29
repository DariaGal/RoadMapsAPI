const axios = require('axios');
const config = require('../config');
require('dotenv').config();

/**
 * Сейчас полученный AccessToken проставляется в заголовк X-Auth-Header (выдуман из головы)
 * После того, как определитесь как с токеном поступать, нужно будет обновить этот метод.
 * Также желательно завести учетку с нормальным логином/паролем (а не TEST/TEST как сейчас)
 */
exports.createClient = async() => {
    var getAuthToken = await axios.post(`${config.apiUrl}auth/signin`, {
        Login: process.env.CLIENTLOGIN,
        Password: process.env.CLIENTPASSWORD
    });

    const clientConfig = {
        baseURL: config.apiUrl,
        headers: {
            'X-Auth-Header': getAuthToken.data.accessToken
        }
    };

    return axios.create(clientConfig);
}