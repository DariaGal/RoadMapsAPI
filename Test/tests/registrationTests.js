const dbHelper = require('../helpers/dbHelper');
const clientHelper = require('../helpers/clientHelper');
const expect = require('expect.js');

describe('Registration tests', () => {
    var client, login, password, response;
    beforeEach(async() => {
        client = await clientHelper.createClient();
        login = 'test-login-' + (Math.floor(Math.random() * 900) + 100).toString();
        password = 'test-password-' + (Math.floor(Math.random() * 900) + 100).toString();
    });

    //В документации 201, на самом деле - 200
    it('Register new user: expect status 201', async() => {
        response = await client.post('auth/signup', {
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