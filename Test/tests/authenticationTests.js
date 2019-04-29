const dbHelper = require('../helpers/dbHelper');
const clientHelper = require('../helpers/clientHelper');
const expect = require('expect.js');
const apiHelper = require('../helpers/apiHelper');

describe('Authentication tests', () => {
    var client, login, password, accessToken, response;
    beforeEach(async() => {
        client = await clientHelper.createClient();//заголовки клиента можно посмотреть так: console.log(client.defaults.headers);
        login = 'test-login-' + (Math.floor(Math.random() * 900) + 100).toString();
        password = 'test-password-' + (Math.floor(Math.random() * 900) + 100).toString();

        accessToken = await apiHelper.registerNewUser(client, login, password);
    });

    //В документации 201, на самом деле - 200
    it('Authenticate by user, which is created: expect status 200', async() => {
        response = await client.post('auth/signin', {
            Login: login,
            Password: password 
        });

        expect(response.status).to.equal(200);
        expect(response.data).to.have.property('accessToken');
        expect(response.data.accessToken).to.be.a('string');
    });

    afterEach(async() => {
        await dbHelper.deleteUserByLogin(login);
    });
});