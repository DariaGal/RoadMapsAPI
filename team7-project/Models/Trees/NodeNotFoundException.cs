using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class NodeNotFoundException : Exception
    {
        public NodeNotFoundException(string nodeId, string treeId)
            : base($"A node by id \"{nodeId}\" is not found in tree by id \"{treeId}\"")
        {
        }
    }
}
