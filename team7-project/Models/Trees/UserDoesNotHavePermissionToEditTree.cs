using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class UserDoesNotHavePermissionToEditTree :Exception
    {
        public UserDoesNotHavePermissionToEditTree(string treeId, string userId)
            : base($"User({userId}) doesn't have permission to edit tree by id \"{treeId}\"")
        {

        }
    }
}
