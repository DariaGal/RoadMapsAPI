using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees.UserTrees
{
    public class UserTreeInfo
    {
        public string Id { get; set; }

        public Guid Author { get; set; }

        public string Title { get; set; }

        public List<string> Tags { get; set; }

        public string Description { get; set; }
    }
}
