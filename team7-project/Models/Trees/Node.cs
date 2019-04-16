using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    /// <summary>
    /// Узел дерева
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Идентификатор узла
        /// </summary>
        [BsonId]
        public string Id { get; set; }

        /// <summary>
        /// Описание узла
        /// </summary>
        [BsonElement("Text")]
        public string Text { get; set; }

        /// <summary>
        /// Координата X узла
        /// </summary>
        [BsonElement("X")]
        public double X { get; set; }

        /// <summary>
        /// Координата Y узла
        /// </summary>
        [BsonElement("Y")]
        public double Y { get; set; }

        /// <summary>
        /// Тип узла
        /// </summary>
        [BsonElement("Type")]
        public string Type { get; set; }
    }
}
