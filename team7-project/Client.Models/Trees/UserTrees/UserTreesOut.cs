using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees.UserTrees
{
    public class UserTreesOut
    {
        public string UserName { get; set; }
        public List<UserTreeInfo> TreesInfo { get; set; }
    }
}
