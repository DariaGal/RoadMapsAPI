using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public static class UserTreesOutConverter
    {
        public static Client.UserTrees.UserTreesOut Convert(string userName, List<Model.UserTrees.UserTreeInfo> userTreeInfo)
        {
            if(userTreeInfo == null)
            {
                throw new ArgumentNullException(nameof(userTreeInfo));
            }

            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var clientTreesInfo = userTreeInfo.Select(x => UserTreeInfoConverter.Convert(x)).ToList();
            var userTreesOut = new Client.UserTrees.UserTreesOut
            {
                UserName = userName,
                TreesInfo = clientTreesInfo
            };

            return userTreesOut;
        }
    }
}
