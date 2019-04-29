const mongoClient = require('mongodb').MongoClient;
const config = require('../config');
require('dotenv').config();

async function createMongoClient() {
    const connectionUrl = `mongodb+srv://${process.env.DBLOGIN}:${process.env.DBPASSWORD}@${config.dbHost}?retryWrites=true`;
    const client = await mongoClient.connect(connectionUrl, { useNewUrlParser: true });

    return client;
}

exports.deleteUserByLogin = async(login) => {
    var result, client, db, users;

    try {
        client = await createMongoClient();
        db = client.db(config.database);
        users = db.collection('Users');

        const usersBeforeDeleting = await users.find({}).toArray();
        console.log(`User's count before deleting: ${usersBeforeDeleting.length}`);

        await users.deleteOne({ Login: login });

        const usersAfterDeleting = await users.find({}).toArray();
        console.log(`User's count after deleting: ${usersAfterDeleting.length}`);
        await client.close();
    } catch (error) {
        console.log(error);
        await client.close();
    }
}