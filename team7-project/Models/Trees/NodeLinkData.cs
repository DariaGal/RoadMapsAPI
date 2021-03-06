﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
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
