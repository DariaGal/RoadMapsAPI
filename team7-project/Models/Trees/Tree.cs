using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    /// <summary>
    /// Дерево
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Идентификатор дерева
        /// </summary>
        [BsonId]
        public string Id { get; set; }

        [BsonElement("AuthorId")]
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Заголовок дерева
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        [BsonElement("Tags")]
        public List<string> Tags { get; set; }


        /// <summary>
        /// Список узлов
        /// </summary>
        [BsonElement("Nodes")]
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Список связей
        /// </summary>
        [BsonElement("Links")]
        public List<Link> Links { get; set; }
    }
}
