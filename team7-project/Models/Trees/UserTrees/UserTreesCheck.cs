using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class UserTreesCheck
    {
        [BsonId]
        public Guid UserId { get; set; }

        [BsonElement("TreeCheck")]
        public List<TreeCheckInfo> TreeCkeck { get; set; }
    }
}
