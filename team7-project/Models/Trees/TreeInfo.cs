using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class TreeInfo
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
    }
}
