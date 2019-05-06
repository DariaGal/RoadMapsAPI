﻿using MongoDB.Bson.Serialization.Attributes;
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

        /// <summary>
        /// Заголовок дерева
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }

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