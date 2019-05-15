using System;
using System.Collections.Generic;
using System.Text;
using Models.Users;

namespace Models.Converters.Users
{

    public static class UserInfoConverter
    {
        public static Client.Models.Users.UserInfo Convert(UserInfo modelUserInfo)
        {
            if(modelUserInfo == null)
            {
                throw new ArgumentNullException(nameof(modelUserInfo));
            }

            var clientUserInfo = new Client.Models.Users.UserInfo
            {
                Login = modelUserInfo.Login
            };

            return clientUserInfo;
        }
    }
}
