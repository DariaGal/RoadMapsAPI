using Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Converters.Users
{
    namespace Models.Converters.Users
    {
        public static class UserConverter
        {
            public static Client.Models.Users.User Convert(User modelUser)
            {
                if (modelUser == null)
                {
                    throw new ArgumentNullException(nameof(modelUser));
                }

                return new Client.Models.Users.User
                {
                    Id = modelUser.Id.ToString(),

                    Login = modelUser.Login,

                    RegisteredAt = modelUser.RegisteredAt
                };
            }

        }
    }
}
