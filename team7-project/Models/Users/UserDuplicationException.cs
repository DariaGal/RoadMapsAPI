using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    /// <summary>
    /// Исключение, которое возникает при попытке создать существующего пользователя
    /// </summary>
    public class UserDuplicationException : Exception
    {
        /// <summary>
        /// Инициализировать эксземпляр исключения по логину пользователя
        /// </summary>
        /// <param name="login"></param>
        public UserDuplicationException(string login)
            : base($"A user with lofgin \"{login}\" is already exist.")
        {
        }
    }
}
