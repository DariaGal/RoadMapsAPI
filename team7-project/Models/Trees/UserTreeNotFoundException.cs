using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class UserTreeNotFoundException : Exception
    {
        /// <summary>
        /// Инициализировать экземпляр исключения по идентификатору дерева
        /// </summary>
        /// <param name="treeId"></param>
        public UserTreeNotFoundException(string treeId, string userId)
            : base($"A tree by id \"{treeId}\" is not found in user({userId}) tree collection")
        {
        }
    }
}
