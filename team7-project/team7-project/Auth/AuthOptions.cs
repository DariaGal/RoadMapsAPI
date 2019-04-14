using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace team7_project.Auth
{
    public class AuthOptions
    {
        //public const string ISSUER = "ToDoListApi"; // издатель токена
        //public const string AUDIENCE = "http://localhost:5001/"; // потребитель токена
        //public const string KEY = " ";   // ключ для шифрации
        public const int LIFETIME = 20; // время жизни токена
    }
}
