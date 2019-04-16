using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    /// <summary>
    /// Связь узлов дерева
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Идетификатор узла источника
        /// </summary>
        [BsonElement("SourceId")]
        public static string SourceId { get; set; }

        /// <summary>
        /// Идетификатор целевого узла
        /// </summary>
        [BsonElement("TargetId")]
        public static string TargetId { get; set; }

        /// <summary>
        /// Тип связи
        /// </summary>
        [BsonElement("Type")]
        public string Type { get; set; }
    }
}
