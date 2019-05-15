using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees
{
    public class TreeInfoSearchQuery
    {
        public int? Offset { get; set; }

        public int? Limit { get; set; }

        public string Title { get; set; }

        public List<string> Tags { get; set; }
    }
}
