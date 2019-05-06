using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees
{
    /// <summary>
    /// Дерево
    /// </summary>
    public class Tree
    {
        public string Id { get; set; }

        /// <summary>
        /// Заголовок дерева
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        public List<string> Tags { get; set; }

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
