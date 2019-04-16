using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{

    /// <summary>
    /// Исключение, которое возникает при попытке получить несуществующее дерево
    /// </summary>
    public class TreeNotFoundException :Exception
    {
        /// <summary>
        /// Инициализировать экземпляр исключения по идентификатору дерева
        /// </summary>
        /// <param name="treeId"></param>
        public TreeNotFoundException(string treeId)
            : base($"A tree by id \"{treeId}\" is not found.")
        {
        }
    }
}
