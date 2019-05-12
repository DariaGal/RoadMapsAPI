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
        public string SourceId { get; set; }

        /// <summary>
        /// Идетификатор целевого узла
        /// </summary>
        [BsonElement("TargetId")]
        public string TargetId { get; set; }
    }
}
