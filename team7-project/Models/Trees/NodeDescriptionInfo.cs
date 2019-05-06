using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class NodeDescriptionInfo
    {
        /// <summary>
        /// Описание узла
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Cgbcjr ccskjr
        /// </summary>
        [BsonElement("NodeData")]
        public List<string> Data { get; set; }


    }

    public class NodeLinkData
    {
        /// <summary>
        /// Описание ссылки
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Текст ссылки
        /// </summary>
        [BsonElement("Link")]
        public string Link { get; set; }
    }
}
