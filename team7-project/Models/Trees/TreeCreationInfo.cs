using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class TreeCreationInfo
    {
        /// <summary>
        /// Заголовок дерева
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Список узлов
        /// </summary>
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Список связей
        /// </summary>
        public List<Link> Links { get; set; }
    }
}
