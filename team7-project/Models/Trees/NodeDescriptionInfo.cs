using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class NodeDescriptionInfo
    {
        /// <summary>
        /// Описание ссылки
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Текст ссылки
        /// </summary>
        [BsonElement("Data")]
        public List<NodeLinkData> Data { get; set; }
    }
}