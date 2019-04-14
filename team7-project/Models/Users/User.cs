using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    public class User
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [BsonElement("Login")]
        public string Login { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [BsonElement("RegisteredAt")]
        public DateTime RegisteredAt { get; set; }
    }
}
