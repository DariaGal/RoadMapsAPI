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
        /// Цвет узла
        /// </summary>
        [BsonElement("Color")]
        public string Color { get; set; }

        /// <summary>
        ///Информация об узле и список ссылок
        /// </summary>
        [BsonElement("Info")]
        public List<NodeDescriptionInfo> Info { get; set; }
    }
}
