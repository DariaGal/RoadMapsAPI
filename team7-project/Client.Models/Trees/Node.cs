using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees
{
    /// <summary>
    /// Узел дерева
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Идентификатор узла
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название узла
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Координата X узла
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Координата Y узла
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Цвет узла
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        ///Информация об узле и список ссылок
        /// </summary>
        public List<NodeDescriptionInfo> Info { get; set; }
    }
}
