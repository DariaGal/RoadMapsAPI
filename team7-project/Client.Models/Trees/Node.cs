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
        /// Описание узла
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Координата X узла
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Координата Y узла
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Тип узла
        /// </summary>
        public string Type { get; set; }
    }
}
